import { useState, useEffect } from 'react';
import { Link } from 'react-router-dom';
import { formApi } from '../api/formApi';
import type { FormDto } from '../types/form';

export default function FormListPage() {
  const [forms, setForms] = useState<FormDto[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);
  const [deletingId, setDeletingId] = useState<string | null>(null);

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

  return (
    <div className="min-h-screen bg-gray-50">
      <div className="max-w-6xl mx-auto px-4 py-8">
        {/* Header */}
        <div className="flex items-center justify-between mb-8">
          <div>
            <h1 className="text-3xl font-bold text-gray-900">Forms</h1>
            <p className="text-gray-500 mt-1">Manage your forms and quizzes</p>
          </div>
          <Link
            to="/forms/new"
            className="flex items-center gap-2 px-5 py-2.5 bg-blue-600 text-white rounded-xl font-semibold hover:bg-blue-700 transition-colors shadow-sm"
          >
            <span>+</span> New Form
          </Link>
        </div>

        {/* Content */}
        {loading ? (
          <div className="flex items-center justify-center py-20">
            <div className="animate-spin w-8 h-8 border-4 border-blue-600 border-t-transparent rounded-full" />
          </div>
        ) : error ? (
          <div className="bg-red-50 border border-red-200 rounded-xl p-6 text-center">
            <p className="text-red-600 font-medium">{error}</p>
            <button onClick={loadForms} className="mt-3 text-sm text-red-500 underline">Try again</button>
          </div>
        ) : forms.length === 0 ? (
          <div className="text-center py-20">
            <div className="text-6xl mb-4">📝</div>
            <h2 className="text-xl font-semibold text-gray-700">No forms yet</h2>
            <p className="text-gray-500 mt-2 mb-6">Create your first form to get started</p>
            <Link
              to="/forms/new"
              className="inline-flex items-center gap-2 px-5 py-2.5 bg-blue-600 text-white rounded-xl font-semibold hover:bg-blue-700 transition-colors"
            >
              Create Form
            </Link>
          </div>
        ) : (
          <div className="grid gap-4">
            {forms.map(form => (
              <div key={form.id} className="bg-white rounded-xl border border-gray-200 shadow-sm hover:shadow-md transition-shadow p-5">
                <div className="flex items-start justify-between gap-4">
                  <div className="flex-1 min-w-0">
                    <div className="flex items-center gap-3 mb-1">
                      <h2 className="text-lg font-semibold text-gray-900 truncate">{form.title}</h2>
                      <span className={`px-2 py-0.5 text-xs font-medium rounded-full ${
                        form.formType === 'Quiz' ? 'bg-purple-100 text-purple-700' : 'bg-blue-100 text-blue-700'
                      }`}>
                        {form.formType}
                      </span>
                      <span className={`px-2 py-0.5 text-xs font-medium rounded-full ${
                        form.isActive ? 'bg-green-100 text-green-700' : 'bg-gray-100 text-gray-500'
                      }`}>
                        {form.isActive ? 'Active' : 'Inactive'}
                      </span>
                    </div>
                    {form.description && (
                      <p className="text-sm text-gray-500 truncate">{form.description}</p>
                    )}
                    <div className="flex items-center gap-4 mt-2 text-xs text-gray-400">
                      <span>{form.questions?.length || 0} questions</span>
                      <span>Audience: {form.audience}</span>
                      {form.timerMinutes && <span>⏱ {form.timerMinutes} min</span>}
                    </div>
                  </div>
                  <div className="flex items-center gap-2 flex-shrink-0">
                    <Link
                      to={`/forms/${form.id}/edit`}
                      className="px-3 py-1.5 text-sm font-medium text-blue-600 border border-blue-200 rounded-lg hover:bg-blue-50 transition-colors"
                    >
                      Edit
                    </Link>
                    <Link
                      to={`/forms/${form.id}/preview`}
                      className="px-3 py-1.5 text-sm font-medium text-green-600 border border-green-200 rounded-lg hover:bg-green-50 transition-colors"
                    >
                      Preview
                    </Link>
                    <Link
                      to={`/forms/${form.id}/responses`}
                      className="px-3 py-1.5 text-sm font-medium text-purple-600 border border-purple-200 rounded-lg hover:bg-purple-50 transition-colors"
                    >
                      Responses
                    </Link>
                    <button
                      onClick={() => handleDelete(form.id)}
                      disabled={deletingId === form.id}
                      className="px-3 py-1.5 text-sm font-medium text-red-600 border border-red-200 rounded-lg hover:bg-red-50 transition-colors disabled:opacity-50"
                    >
                      {deletingId === form.id ? '...' : 'Delete'}
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
