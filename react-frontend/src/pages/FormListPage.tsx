import { useState, useEffect } from 'react';
import { Link } from 'react-router-dom';
import { formApi } from '../api/formApi';
import type { FormDto } from '../types/form';

export default function FormListPage() {
  const [forms, setForms] = useState<FormDto[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);
  const [deletingId, setDeletingId] = useState<string | null>(null);
  const [search, setSearch] = useState('');

  useEffect(() => {
    loadForms();
  }, []);

  const loadForms = async () => {
    try {
      setLoading(true);
      setError(null);
      const result = await formApi.list();
      setForms(result.items || []);
    } catch {
      setError('Failed to load forms. Make sure the backend is running.');
      setForms([]);
    } finally {
      setLoading(false);
    }
  };

  const handleDelete = async (id: string) => {
    if (!confirm('Are you sure you want to delete this form?')) return;
    setDeletingId(id);
    try {
      await formApi.delete(id);
      setForms(forms.filter(f => f.id !== id));
    } catch {
      alert('Failed to delete form');
    } finally {
      setDeletingId(null);
    }
  };

  const filtered = forms.filter(f =>
    f.title.toLowerCase().includes(search.toLowerCase()) ||
    (f.description || '').toLowerCase().includes(search.toLowerCase())
  );

  return (
    <div className="min-h-screen bg-gray-50">
      {/* App header */}
      <header className="bg-white border-b border-gray-200 sticky top-0 z-30">
        <div className="max-w-6xl mx-auto px-4 py-3 flex items-center justify-between">
          <div className="flex items-center gap-3">
            <div className="w-9 h-9 rounded-xl bg-gradient-to-br from-blue-600 to-indigo-600 flex items-center justify-center text-white font-bold text-lg shadow-sm select-none">
              A
            </div>
            <div>
              <h1 className="text-base font-bold text-gray-900 leading-tight">AlAshmar Forms</h1>
              <p className="text-xs text-gray-400 leading-tight">Form & Quiz Builder</p>
            </div>
          </div>
          <Link
            to="/forms/new"
            className="flex items-center gap-1.5 px-4 py-2 bg-blue-600 text-white rounded-xl font-semibold text-sm hover:bg-blue-700 active:bg-blue-800 transition-colors shadow-sm"
          >
            <span className="text-base leading-none">+</span>
            New Form
          </Link>
        </div>
      </header>

      <div className="max-w-6xl mx-auto px-4 py-8">
        {/* Page title + search */}
        <div className="flex flex-col sm:flex-row sm:items-center justify-between gap-4 mb-6">
          <div>
            <h2 className="text-2xl font-bold text-gray-900">My Forms</h2>
            {!loading && !error && (
              <p className="text-sm text-gray-500 mt-0.5">
                {forms.length} {forms.length === 1 ? 'form' : 'forms'} total
              </p>
            )}
          </div>
          {forms.length > 0 && (
            <div className="relative">
              <span className="absolute left-3 top-1/2 -translate-y-1/2 text-gray-400 text-sm pointer-events-none">🔍</span>
              <input
                type="text"
                value={search}
                onChange={e => setSearch(e.target.value)}
                placeholder="Search forms..."
                className="pl-9 pr-4 py-2 border border-gray-300 rounded-xl text-sm focus:ring-2 focus:ring-blue-500 focus:border-blue-500 outline-none bg-white w-56 transition-shadow"
              />
            </div>
          )}
        </div>

        {/* Content */}
        {loading ? (
          <div className="grid gap-4 sm:grid-cols-2 lg:grid-cols-3">
            {[...Array(3)].map((_, i) => (
              <div key={i} className="bg-white rounded-2xl border border-gray-200 p-5 animate-pulse">
                <div className="flex gap-2 mb-4">
                  <div className="h-5 bg-gray-200 rounded-full w-14" />
                  <div className="h-5 bg-gray-100 rounded-full w-16" />
                </div>
                <div className="h-4 bg-gray-200 rounded w-3/4 mb-2" />
                <div className="h-3 bg-gray-100 rounded w-1/2 mb-6" />
                <div className="grid grid-cols-2 gap-2">
                  <div className="h-8 bg-gray-100 rounded-lg" />
                  <div className="h-8 bg-gray-100 rounded-lg" />
                  <div className="h-8 bg-gray-100 rounded-lg" />
                  <div className="h-8 bg-gray-100 rounded-lg" />
                </div>
              </div>
            ))}
          </div>
        ) : error ? (
          <div className="bg-red-50 border border-red-200 rounded-2xl p-10 text-center">
            <div className="text-4xl mb-3">⚠️</div>
            <p className="text-red-700 font-semibold mb-1">Connection Error</p>
            <p className="text-red-500 text-sm mb-5">{error}</p>
            <button
              onClick={loadForms}
              className="px-5 py-2 text-sm font-medium text-red-600 border border-red-300 rounded-xl hover:bg-red-50 transition-colors"
            >
              Try again
            </button>
          </div>
        ) : forms.length === 0 ? (
          <div className="flex flex-col items-center py-20 text-center">
            <div className="w-20 h-20 rounded-3xl bg-blue-50 flex items-center justify-center text-4xl mb-5 shadow-inner">
              📝
            </div>
            <h2 className="text-xl font-semibold text-gray-800">No forms yet</h2>
            <p className="text-gray-500 mt-2 mb-7 text-sm max-w-xs">
              Create your first form or quiz to start collecting responses from students and teachers.
            </p>
            <Link
              to="/forms/new"
              className="inline-flex items-center gap-2 px-5 py-2.5 bg-blue-600 text-white rounded-xl font-semibold text-sm hover:bg-blue-700 transition-colors shadow-sm"
            >
              + Create your first form
            </Link>
          </div>
        ) : filtered.length === 0 ? (
          <div className="text-center py-16">
            <div className="text-4xl mb-3">🔍</div>
            <h2 className="text-lg font-semibold text-gray-700">No results for &ldquo;{search}&rdquo;</h2>
            <button onClick={() => setSearch('')} className="mt-2 text-sm text-blue-600 hover:underline">
              Clear search
            </button>
          </div>
        ) : (
          <div className="grid gap-4 sm:grid-cols-2 lg:grid-cols-3">
            {filtered.map(form => (
              <div
                key={form.id}
                className="bg-white rounded-2xl border border-gray-200 shadow-sm hover:shadow-md hover:-translate-y-0.5 transition-all duration-200 flex flex-col overflow-hidden"
              >
                {/* Colored accent bar */}
                <div
                  className="h-1.5 flex-shrink-0"
                  style={{ backgroundColor: form.primaryColor || (form.formType === 'Quiz' ? '#8b5cf6' : '#3b82f6') }}
                />
                <div className="p-5 flex flex-col flex-1">
                  {/* Badges */}
                  <div className="flex items-center gap-2 mb-3">
                    <span className={`inline-flex items-center gap-1 px-2.5 py-0.5 text-xs font-semibold rounded-full ${
                      form.formType === 'Quiz' ? 'bg-purple-100 text-purple-700' : 'bg-blue-100 text-blue-700'
                    }`}>
                      {form.formType === 'Quiz' ? '🏆' : '📋'} {form.formType}
                    </span>
                    <span className={`inline-flex items-center gap-1 px-2.5 py-0.5 text-xs font-semibold rounded-full ${
                      form.isActive ? 'bg-emerald-100 text-emerald-700' : 'bg-gray-100 text-gray-500'
                    }`}>
                      <span className="text-[8px]">{form.isActive ? '●' : '○'}</span>
                      {form.isActive ? 'Active' : 'Inactive'}
                    </span>
                  </div>

                  {/* Title & description */}
                  <h3 className="text-base font-semibold text-gray-900 leading-snug line-clamp-2 mb-1">
                    {form.title}
                  </h3>
                  {form.description && (
                    <p className="text-sm text-gray-500 line-clamp-2 mb-2">{form.description}</p>
                  )}

                  {/* Meta info */}
                  <div className="flex items-center gap-3 mt-auto pt-3 mb-4 border-t border-gray-100 text-xs text-gray-400">
                    <span title="Questions">📝 {form.questions?.length || 0}</span>
                    <span title="Audience">👥 {form.audience}</span>
                    {form.timerMinutes && <span title="Timer">⏱ {form.timerMinutes}m</span>}
                  </div>

                  {/* Action buttons */}
                  <div className="grid grid-cols-2 gap-2">
                    <Link
                      to={`/forms/${form.id}/edit`}
                      className="flex items-center justify-center gap-1.5 py-2 text-xs font-semibold text-blue-700 bg-blue-50 rounded-xl hover:bg-blue-100 transition-colors"
                    >
                      ✏️ Edit
                    </Link>
                    <Link
                      to={`/forms/${form.id}/preview`}
                      className="flex items-center justify-center gap-1.5 py-2 text-xs font-semibold text-gray-600 bg-gray-50 rounded-xl hover:bg-gray-100 transition-colors"
                    >
                      👁 Preview
                    </Link>
                    <Link
                      to={`/forms/${form.id}/responses`}
                      className="flex items-center justify-center gap-1.5 py-2 text-xs font-semibold text-purple-700 bg-purple-50 rounded-xl hover:bg-purple-100 transition-colors"
                    >
                      📊 Responses
                    </Link>
                    <button
                      onClick={() => handleDelete(form.id)}
                      disabled={deletingId === form.id}
                      className="flex items-center justify-center gap-1.5 py-2 text-xs font-semibold text-red-600 bg-red-50 rounded-xl hover:bg-red-100 transition-colors disabled:opacity-50"
                    >
                      {deletingId === form.id ? '…' : '🗑 Delete'}
                    </button>
                  </div>
                </div>
              </div>
            ))}
          </div>
        )}
      </div>
    </div>
  );
}

