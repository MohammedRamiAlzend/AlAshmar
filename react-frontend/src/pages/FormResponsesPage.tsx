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

  if (loading) return (
    <div className="min-h-screen flex items-center justify-center">
      <div className="animate-spin w-8 h-8 border-4 border-blue-600 border-t-transparent rounded-full" />
    </div>
  );

  return (
    <div className="min-h-screen bg-gray-50">
      <div className="max-w-5xl mx-auto px-4 py-8">
        {/* Header */}
        <div className="flex items-center justify-between mb-8">
          <div>
            <div className="flex items-center gap-3 mb-1">
              <Link to="/" className="text-gray-500 hover:text-gray-700 text-sm">← Back</Link>
              <span className="text-gray-300">|</span>
              <h1 className="text-2xl font-bold text-gray-900">Responses</h1>
            </div>
            <p className="text-gray-500">{form?.title}</p>
          </div>
          <div className="text-right">
            <div className="text-3xl font-bold text-blue-600">{responses.length}</div>
            <div className="text-sm text-gray-500">Total Responses</div>
          </div>
        </div>

        {error && (
          <div className="bg-red-50 border border-red-200 rounded-xl p-4 text-red-600 mb-6">{error}</div>
        )}

        {responses.length === 0 ? (
          <div className="text-center py-16">
            <div className="text-5xl mb-4">📭</div>
            <h2 className="text-xl font-semibold text-gray-700">No responses yet</h2>
            <p className="text-gray-500 mt-2">Share the form to start collecting responses</p>
            {form?.accessToken && (
              <div className="mt-4 flex items-center justify-center gap-2">
                <code className="bg-gray-100 px-4 py-2 rounded-lg text-sm text-gray-700">
                  /fill/{form.accessToken}
                </code>
                <button
                  onClick={() => navigator.clipboard.writeText(`${window.location.origin}/fill/${form?.accessToken}`)}
                  className="px-3 py-2 text-sm bg-blue-600 text-white rounded-lg hover:bg-blue-700"
                >
                  Copy Link
                </button>
              </div>
            )}
          </div>
        ) : (
          <div className="space-y-3">
            {responses.map((response, idx) => (
              <div key={response.id} className="bg-white rounded-xl border border-gray-200 shadow-sm">
                <div
                  className="flex items-center justify-between p-4 cursor-pointer hover:bg-gray-50 rounded-xl"
                  onClick={() => setExpandedId(expandedId === response.id ? null : response.id)}
                >
                  <div className="flex items-center gap-4">
                    <div className="w-8 h-8 rounded-full bg-blue-100 text-blue-700 font-semibold text-sm flex items-center justify-center">
                      {idx + 1}
                    </div>
                    <div>
                      <div className="font-medium text-gray-900">
                        {response.respondedByStudentId
                          ? `Student ${response.respondedByStudentId.substring(0, 8)}`
                          : response.respondedByTeacherId
                          ? `Teacher ${response.respondedByTeacherId.substring(0, 8)}`
                          : 'Anonymous'}
                      </div>
                      <div className="text-xs text-gray-500">
                        {new Date(response.submittedAt).toLocaleDateString()} at {new Date(response.submittedAt).toLocaleTimeString()}
                        {response.timeSpentSeconds != null && ` • ${Math.round(response.timeSpentSeconds / 60)} min`}
                      </div>
                    </div>
                  </div>
                  <div className="flex items-center gap-4">
                    {form?.formType === 'Quiz' && response.score !== undefined && (
                      <div className="text-right">
                        <div className="text-lg font-bold text-green-600">{response.score}</div>
                        <div className="text-xs text-gray-500">points</div>
                      </div>
                    )}
                    <span className="text-gray-400">{expandedId === response.id ? '▲' : '▼'}</span>
                  </div>
                </div>

                {expandedId === response.id && (
                  <div className="px-4 pb-4 border-t border-gray-100">
                    <div className="mt-3 space-y-3">
                      {(response.answers || []).map(answer => (
                        <div key={answer.id} className="bg-gray-50 rounded-lg p-3">
                          <div className="text-sm font-medium text-gray-700 mb-1">{answer.questionText}</div>
                          {answer.textAnswer && (
                            <div className="text-sm text-gray-600">{answer.textAnswer}</div>
                          )}
                          {answer.selectedOptions && answer.selectedOptions.length > 0 && (
                            <div className="flex flex-wrap gap-1">
                              {answer.selectedOptions.map(opt => (
                                <span key={opt.id} className="px-2 py-0.5 text-xs bg-blue-100 text-blue-700 rounded-full">
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
