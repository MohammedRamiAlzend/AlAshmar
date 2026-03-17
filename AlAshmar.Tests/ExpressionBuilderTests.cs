using ExpressionBuilderLib.src.Core;
using ExpressionBuilderLib.src.Core.Enums;

namespace AlAshmar.Tests;

public class ExpressionBuilderTests
{
    /// <summary>A simple test entity used only in these tests.</summary>
    private sealed class Person
    {
        public string Name { get; init; } = string.Empty;
        public int Age { get; init; }
        public bool IsActive { get; init; }
    }

    private static readonly List<Person> _people =
    [
        new() { Name = "Alice", Age = 30, IsActive = true },
        new() { Name = "Bob",   Age = 17, IsActive = true },
        new() { Name = "Carol", Age = 25, IsActive = false },
        new() { Name = "Dave",  Age = 40, IsActive = true },
    ];

    // ── And ──────────────────────────────────────────────────────────────────

    [Fact]
    public void And_CombinesTwoPredicates_ReturnsOnlyMatchingBoth()
    {
        var builder = new ExpressionBuilder<Person>()
            .And(p => p.Age >= 18)
            .And(p => p.IsActive);

        var fn = builder.Compile();
        var result = _people.Where(fn).ToList();

        Assert.Equal(2, result.Count);
        Assert.All(result, p => Assert.True(p.Age >= 18 && p.IsActive));
    }

    // ── Or ───────────────────────────────────────────────────────────────────

    [Fact]
    public void Or_ReturnsItemsMatchingEitherPredicate()
    {
        var builder = new ExpressionBuilder<Person>()
            .And(p => p.Age < 18)
            .Or(p => p.Name == "Carol");

        var fn = builder.Compile();
        var result = _people.Where(fn).ToList();

        // Bob (age < 18) + Carol (name match)
        Assert.Equal(2, result.Count);
        Assert.Contains(result, p => p.Name == "Bob");
        Assert.Contains(result, p => p.Name == "Carol");
    }

    // ── AndNot ───────────────────────────────────────────────────────────────

    [Fact]
    public void AndNot_ExcludesMatchingItems()
    {
        var builder = new ExpressionBuilder<Person>()
            .And(p => p.IsActive)
            .AndNot(p => p.Age < 18);

        var fn = builder.Compile();
        var result = _people.Where(fn).ToList();

        // Active AND NOT under-18  →  Alice, Dave
        Assert.Equal(2, result.Count);
        Assert.DoesNotContain(result, p => p.Age < 18);
    }

    // ── OrNot ────────────────────────────────────────────────────────────────

    [Fact]
    public void OrNot_IncludesItemsWherePredicateIsFalse()
    {
        // Start with Age < 18, OR NOT IsActive  →  Bob OR Carol
        var builder = new ExpressionBuilder<Person>()
            .And(p => p.Age < 18)
            .OrNot(p => p.IsActive);

        var fn = builder.Compile();
        var result = _people.Where(fn).ToList();

        // Bob (age < 18) and Carol (not active)
        Assert.Equal(2, result.Count);
        Assert.Contains(result, p => p.Name == "Bob");
        Assert.Contains(result, p => p.Name == "Carol");
    }

    // ── Build ─────────────────────────────────────────────────────────────────

    [Fact]
    public void Build_ReturnsValidExpression()
    {
        var builder = new ExpressionBuilder<Person>()
            .And(p => p.Name.StartsWith("A"));

        var expr = builder.Build();

        Assert.NotNull(expr);
        var fn = expr.Compile();
        Assert.True(fn(new Person { Name = "Alice", Age = 30, IsActive = true }));
        Assert.False(fn(new Person { Name = "Bob",   Age = 17, IsActive = true }));
    }

    // ── Compile ───────────────────────────────────────────────────────────────

    [Fact]
    public void Compile_ReturnsWorkingDelegate()
    {
        var builder = new ExpressionBuilder<Person>()
            .And(p => p.Age > 20);

        Func<Person, bool> fn = builder.Compile();

        Assert.True(fn(new Person { Name = "X", Age = 25, IsActive = true }));
        Assert.False(fn(new Person { Name = "Y", Age = 18, IsActive = true }));
    }

    // ── Reset ─────────────────────────────────────────────────────────────────

    [Fact]
    public void Reset_ClearsBackToAlwaysTruePredicate()
    {
        var builder = new ExpressionBuilder<Person>()
            .And(p => p.Age > 100); // nothing matches

        builder.Reset();

        var fn = builder.Compile();
        // After reset, everything should pass
        Assert.All(_people, p => Assert.True(fn(p)));
    }

    // ── Clone ─────────────────────────────────────────────────────────────────

    [Fact]
    public void Clone_ProducesIndependentCopy()
    {
        var original = new ExpressionBuilder<Person>()
            .And(p => p.IsActive);

        var clone = original.Clone();

        // Mutate the clone — original must not be affected
        clone.And(p => p.Age > 35);

        var originalFn = original.Compile();
        var cloneFn    = clone.Compile();

        // Original: active people → Alice, Bob, Dave  (3)
        var originalResult = _people.Where(originalFn).ToList();
        Assert.Equal(3, originalResult.Count);

        // Clone: active AND age > 35 → Dave only  (1)
        var cloneResult = _people.Where(cloneFn).ToList();
        Assert.Single(cloneResult);
        Assert.Equal("Dave", cloneResult[0].Name);
    }

    // ── Implicit conversion to Expression<Func<T, bool>> ──────────────────────

    [Fact]
    public void ImplicitConversion_ToExpression_Works()
    {
        var builder = new ExpressionBuilder<Person>()
            .And(p => p.Name == "Alice");

        Expression<Func<Person, bool>> expr = builder; // implicit cast

        Assert.NotNull(expr);
        var fn = expr.Compile();
        Assert.True(fn(_people[0]));   // Alice
        Assert.False(fn(_people[1]));  // Bob
    }

    // ── Chaining multiple conditions ──────────────────────────────────────────

    [Fact]
    public void Chaining_MultipleConditions_WorksCorrectly()
    {
        var builder = new ExpressionBuilder<Person>()
            .And(p => p.IsActive)
            .And(p => p.Age >= 18)
            .And(p => p.Name.Length > 3);

        var fn = builder.Compile();
        var result = _people.Where(fn).ToList();

        // Active, adult, name longer than 3 chars → Alice (5), Dave (4)
        Assert.Equal(2, result.Count);
        Assert.All(result, p =>
        {
            Assert.True(p.IsActive);
            Assert.True(p.Age >= 18);
            Assert.True(p.Name.Length > 3);
        });
    }

    // ── AddCondition with LogicalOperator enum ───────────────────────────────

    [Fact]
    public void AddCondition_WithOrOperator_BehavesLikeOr()
    {
        var builder = new ExpressionBuilder<Person>()
            .And(p => p.Age < 20)
            .AddCondition(p => p.Name == "Carol", LogicalOperator.Or);

        var fn = builder.Compile();
        var result = _people.Where(fn).ToList();

        Assert.Equal(2, result.Count);
        Assert.Contains(result, p => p.Name == "Bob");
        Assert.Contains(result, p => p.Name == "Carol");
    }
}
