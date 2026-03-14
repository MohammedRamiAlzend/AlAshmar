using System.Text;
using ExpressionBuilderLib.src.Core.Enums;
using ExpressionBuilderLib.src.Core.Interfaces;

namespace ExpressionBuilderLib.src.Core;





    public class ExpressionBuilder<T> : IExpressionBuilder<T>
    {
        private Expression<Func<T, bool>> _expression;
        private readonly ParameterExpression _parameter;




        public ExpressionBuilder()
        {
            _parameter = Expression.Parameter(typeof(T), "x");
            _expression = Expression.Lambda<Func<T, bool>>(Expression.Constant(true), _parameter);
        }





        public ExpressionBuilder(Expression<Func<T, bool>> initialExpression)
        {
            _parameter = initialExpression.Parameters[0];
            _expression = initialExpression;
        }






        public ExpressionBuilder<T> And(Expression<Func<T, bool>> predicate)
        {
            if (predicate == null) return this;

            var combined = ExpressionCombiner.Combine(
                _expression,
                predicate,
                LogicalOperator.And);

            _expression = combined;
            return this;
        }






        public ExpressionBuilder<T> Or(Expression<Func<T, bool>> predicate)
        {
            if (predicate == null) return this;

            var combined = ExpressionCombiner.Combine(
                _expression,
                predicate,
                LogicalOperator.Or);

            _expression = combined;
            return this;
        }






        public ExpressionBuilder<T> AndNot(Expression<Func<T, bool>> predicate)
        {
            if (predicate == null) return this;

            var combined = ExpressionCombiner.Combine(
                _expression,
                predicate,
                LogicalOperator.AndNot);

            _expression = combined;
            return this;
        }






        public ExpressionBuilder<T> OrNot(Expression<Func<T, bool>> predicate)
        {
            if (predicate == null) return this;

            var combined = ExpressionCombiner.Combine(
                _expression,
                predicate,
                LogicalOperator.OrNot);

            _expression = combined;
            return this;
        }







        public ExpressionBuilder<T> AddCondition(
            Expression<Func<T, bool>> predicate,
            LogicalOperator logicalOperator = LogicalOperator.And)
        {
            return logicalOperator switch
            {
                LogicalOperator.And => And(predicate),
                LogicalOperator.Or => Or(predicate),
                LogicalOperator.AndNot => AndNot(predicate),
                LogicalOperator.OrNot => OrNot(predicate),
                _ => And(predicate)
            };
        }





        public Expression<Func<T, bool>> Build()
        {
            return _expression;
        }





        public Func<T, bool> Compile()
        {
            return _expression.Compile();
        }




        public void Reset()
        {
            _expression = Expression.Lambda<Func<T, bool>>(
                Expression.Constant(true),
                _parameter);
        }





        public ExpressionBuilder<T> Clone()
        {
            return new ExpressionBuilder<T>(_expression);
        }





        public override string ToString()
        {
            return _expression?.ToString() ?? string.Empty;
        }




        public static implicit operator Expression<Func<T, bool>>(ExpressionBuilder<T> builder)
        {
            return builder?.Build();
        }




        public static implicit operator Func<T, bool>(ExpressionBuilder<T> builder)
        {
            return builder?.Compile();
        }
}
