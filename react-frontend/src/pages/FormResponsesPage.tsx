import { useState, useEffect } from 'react';
import { useParams, Link } from 'react-router-dom';
import { formApi, responseApi } from '../api/formApi';
import type { FormDto, FormResponseDto } from '../types/form';

export default function FormResponsesPage() {
  const { id } = useParams<{ id: string }>();
  const [form, setForm] = useState<FormDto | null>(null);
  const [responses, setResponses] = useState<FormResponseDto[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);
  const [expandedId, setExpandedId] = useState<string | null>(null);
  const [copied, setCopied] = useState(false);

  useEffect(() => {
    if (id) {
      Promise.all([
        formApi.get(id),
        responseApi.list(id),
      ])
        .then(([f, r]) => {
          setForm(f);
          setResponses(r);
        })
        .catch(() => setError('Failed to load responses'))
        .finally(() => setLoading(false));
    }
  }, [id]);

  const copyLink = () => {
    if (!form?.accessToken) return;
    navigator.clipboard.writeText(`${window.location.origin}/fill/${form.accessToken}`);
    setCopied(true);
    setTimeout(() => setCopied(false), 2000);
  };

  const avgScore = responses.length > 0 && form?.formType === 'Quiz'
    ? Math.round(responses.reduce((sum, r) => sum + (r.score ?? 0), 0) / responses.length)
    : null;

  const avgMinutes = responses.length > 0
    ? Math.round(responses.reduce((sum, r) => sum + (r.timeSpentSeconds ?? 0), 0) / responses.length / 60)
    : null;

  if (loading) return (
    <div className="min-h-screen flex items-center justify-center bg-gray-50">
      <div className="animate-spin w-8 h-8 border-4 border-blue-600 border-t-transparent rounded-full" />
    </div>
  );

  return (
    <div className="min-h-screen bg-gray-50">
      {/* Header */}
      <header className="bg-white border-b border-gray-200 sticky top-0 z-30">
        <div className="max-w-5xl mx-auto px-4 py-3 flex items-center gap-4">
          <Link to="/" className="text-gray-500 hover:text-gray-700 text-sm font-medium">← Back</Link>
          <span className="text-gray-200">|</span>
          <div className="flex-1 min-w-0">
            <h1 className="text-base font-bold text-gray-900 truncate">{form?.title || 'Responses'}</h1>
            <p className="text-xs text-gray-400">Response analytics</p>
          </div>
          {form?.accessToken && (
            <button
              onClick={copyLink}
              className={`flex items-center gap-1.5 px-3 py-1.5 text-xs font-semibold rounded-xl border transition-all ${
                copied
                  ? 'bg-green-50 border-green-300 text-green-700'
                  : 'bg-blue-50 border-blue-200 text-blue-700 hover:bg-blue-100'
              }`}
            >
              {copied ? '✅ Copied!' : '🔗 Copy Share Link'}
            </button>
          )}
        </div>
      </header>

      <div className="max-w-5xl mx-auto px-4 py-8">
        {error && (
          <div className="bg-red-50 border border-red-200 rounded-2xl p-4 text-red-600 mb-6">{error}</div>
        )}

        {/* Stats cards */}
        <div className="grid grid-cols-2 sm:grid-cols-4 gap-4 mb-8">
          <div className="bg-white rounded-2xl border border-gray-200 p-4 text-center shadow-sm">
            <div className="text-3xl font-bold text-blue-600">{responses.length}</div>
            <div className="text-xs text-gray-500 mt-1 font-medium">Total Responses</div>
          </div>
          <div className="bg-white rounded-2xl border border-gray-200 p-4 text-center shadow-sm">
            <div className="text-3xl font-bold text-gray-700">{form?.questions?.length || 0}</div>
            <div className="text-xs text-gray-500 mt-1 font-medium">Questions</div>
          </div>
          {avgScore !== null ? (
            <div className="bg-white rounded-2xl border border-gray-200 p-4 text-center shadow-sm">
              <div className="text-3xl font-bold text-green-600">{avgScore}</div>
              <div className="text-xs text-gray-500 mt-1 font-medium">Avg Score</div>
            </div>
          ) : (
            <div className="bg-white rounded-2xl border border-gray-200 p-4 text-center shadow-sm opacity-40">
              <div className="text-3xl font-bold text-gray-400">—</div>
              <div className="text-xs text-gray-400 mt-1 font-medium">Avg Score</div>
            </div>
          )}
          <div className="bg-white rounded-2xl border border-gray-200 p-4 text-center shadow-sm">
            <div className="text-3xl font-bold text-purple-600">{avgMinutes ?? '—'}</div>
            <div className="text-xs text-gray-500 mt-1 font-medium">Avg Minutes</div>
          </div>
        </div>

        {responses.length === 0 ? (
          <div className="text-center py-16 bg-white rounded-2xl border border-gray-200 shadow-sm">
            <div className="text-5xl mb-4">📭</div>
            <h2 className="text-xl font-semibold text-gray-700">No responses yet</h2>
            <p className="text-gray-500 mt-2 mb-5 text-sm">Share the form link to start collecting responses</p>
            {form?.accessToken && (
              <div className="flex items-center justify-center gap-2">
                <code className="bg-gray-100 px-4 py-2 rounded-xl text-sm text-gray-700 font-mono">
                  /fill/{form.accessToken}
                </code>
                <button
                  onClick={copyLink}
                  className={`px-3 py-2 text-sm font-semibold rounded-xl transition-all ${
                    copied ? 'bg-green-600 text-white' : 'bg-blue-600 text-white hover:bg-blue-700'
                  }`}
                >
                  {copied ? '✅ Copied!' : 'Copy Link'}
                </button>
              </div>
            )}
          </div>
        ) : (
          <div className="space-y-3">
            {responses.map((response, idx) => (
              <div key={response.id} className="bg-white rounded-2xl border border-gray-200 shadow-sm overflow-hidden">
                <div
                  className="flex items-center justify-between px-5 py-4 cursor-pointer hover:bg-gray-50 transition-colors"
                  onClick={() => setExpandedId(expandedId === response.id ? null : response.id)}
                >
                  <div className="flex items-center gap-4">
                    <div className="w-9 h-9 rounded-full bg-gradient-to-br from-blue-100 to-indigo-100 text-blue-700 font-bold text-sm flex items-center justify-center flex-shrink-0">
                      {idx + 1}
                    </div>
                    <div>
                      <div className="font-semibold text-gray-900 text-sm">
                        {response.respondedByStudentId
                          ? `Student · ${response.respondedByStudentId.substring(0, 8)}…`
                          : response.respondedByTeacherId
                          ? `Teacher · ${response.respondedByTeacherId.substring(0, 8)}…`
                          : 'Anonymous'}
                      </div>
                      <div className="text-xs text-gray-400 mt-0.5">
                        {new Date(response.submittedAt).toLocaleDateString()} at {new Date(response.submittedAt).toLocaleTimeString()}
                        {response.timeSpentSeconds != null && ` · ${Math.round(response.timeSpentSeconds / 60)} min`}
                      </div>
                    </div>
                  </div>
                  <div className="flex items-center gap-4">
                    {form?.formType === 'Quiz' && response.score !== undefined && (
                      <div className="text-right">
                        <div className="text-lg font-bold text-green-600">{response.score} pts</div>
                      </div>
                    )}
                    <span className="text-gray-400 text-sm">{expandedId === response.id ? '▲' : '▼'}</span>
                  </div>
                </div>

                {expandedId === response.id && (
                  <div className="px-5 pb-5 border-t border-gray-100">
                    <div className="mt-4 space-y-3">
                      {(response.answers || []).map(answer => (
                        <div key={answer.id} className="bg-gray-50 rounded-xl p-4">
                          <div className="text-sm font-semibold text-gray-700 mb-2">{answer.questionText}</div>
                          {answer.textAnswer && (
                            <div className="text-sm text-gray-600 bg-white rounded-lg px-3 py-2 border border-gray-200">{answer.textAnswer}</div>
                          )}
                          {answer.selectedOptions && answer.selectedOptions.length > 0 && (
                            <div className="flex flex-wrap gap-1.5">
                              {answer.selectedOptions.map(opt => (
                                <span key={opt.id} className="px-3 py-1 text-xs font-medium bg-blue-100 text-blue-700 rounded-full">
                                  {opt.text}
                                </span>
                              ))}
                            </div>
                          )}
                        </div>
                      ))}
                    </div>
                  </div>
                )}
              </div>
            ))}
          </div>
        )}
      </div>
    </div>
  );
}

