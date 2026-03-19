import { useState, useEffect } from 'react';
import { semesterApi, courseApi } from '../api/academicApi';
import type { SemesterDto, CourseDto } from '../types/academic';
import MainLayout from '../components/MainLayout';
import { useT } from '../i18n';

interface CourseFormState {
  eventName: string;
  semesterId: string;
}

const emptyForm = (): CourseFormState => ({ eventName: '', semesterId: '' });

export default function CourseListPage() {
  const t = useT();
  const [semesters, setSemesters] = useState<SemesterDto[]>([]);
  const [courses, setCourses] = useState<CourseDto[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);
  const [saving, setSaving] = useState(false);
  const [filterSemesterId, setFilterSemesterId] = useState('');

  // null = closed, undefined = new, string = editing id
  const [editingId, setEditingId] = useState<string | null | undefined>(null);
  const [form, setForm] = useState<CourseFormState>(emptyForm());
  const [deletingId, setDeletingId] = useState<string | null>(null);

  useEffect(() => {
    loadAll();
  }, []);

  const loadAll = async () => {
    try {
      setLoading(true);
      setError(null);
      const [sem, crs] = await Promise.all([semesterApi.list(), courseApi.list()]);
      setSemesters(sem);
      setCourses(crs);
    } catch {
      setError(t.loadError);
    } finally {
      setLoading(false);
    }
  };

  const openAdd = () => {
    setForm({ eventName: '', semesterId: filterSemesterId || (semesters[0]?.id ?? '') });
    setEditingId(undefined);
  };

  const openEdit = (c: CourseDto) => {
    setForm({ eventName: c.eventName, semesterId: c.semesterId });
    setEditingId(c.id);
  };

  const closeForm = () => setEditingId(null);

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setSaving(true);
    try {
      if (editingId === undefined) {
        const created = await courseApi.create(form);
        setCourses(prev => [...prev, created]);
      } else if (editingId) {
        const updated = await courseApi.update(editingId, { eventName: form.eventName });
        setCourses(prev => prev.map(c => c.id === editingId ? updated : c));
      }
      closeForm();
    } catch {
      alert(t.connectionError);
    } finally {
      setSaving(false);
    }
  };

  const handleDelete = async (id: string) => {
    if (!confirm(t.confirmDelete)) return;
    setDeletingId(id);
    try {
      await courseApi.delete(id);
      setCourses(prev => prev.filter(c => c.id !== id));
    } catch {
      alert(t.deleteError);
    } finally {
      setDeletingId(null);
    }
  };

  const filtered = filterSemesterId
    ? courses.filter(c => c.semesterId === filterSemesterId)
    : courses;

  const isFormOpen = editingId !== null;

  return (
    <MainLayout
      headerContent={
        <div className="flex justify-end w-full">
          <button
            onClick={openAdd}
            className="flex items-center gap-1.5 px-4 py-2 bg-blue-600 text-white rounded-xl font-semibold text-sm hover:bg-blue-700 active:bg-blue-800 transition-colors shadow-sm"
          >
            {t.addCourse}
          </button>
        </div>
      }
    >
      <div className="max-w-4xl mx-auto px-4 py-8">
        <div className="flex flex-col sm:flex-row sm:items-center justify-between gap-4 mb-6">
          <div>
            <h2 className="text-2xl font-bold text-gray-900 dark:text-white">{t.courseManagement}</h2>
            {!loading && !error && (
              <p className="text-sm text-gray-500 dark:text-slate-400 mt-0.5">
                {filtered.length} {t.courses.toLowerCase()}
              </p>
            )}
          </div>
          {semesters.length > 0 && (
            <select
              value={filterSemesterId}
              onChange={e => setFilterSemesterId(e.target.value)}
              className="px-3 py-2 border border-gray-300 dark:border-slate-600 rounded-xl text-sm bg-white dark:bg-slate-800 text-gray-900 dark:text-white focus:ring-2 focus:ring-blue-500 focus:border-blue-500 outline-none transition-all"
            >
              <option value="">{t.allSemesters}</option>
              {semesters.map(s => (
                <option key={s.id} value={s.id}>{s.name}</option>
              ))}
            </select>
          )}
        </div>

        {/* Inline Add/Edit form */}
        {isFormOpen && (
          <div className="bg-white dark:bg-slate-800 rounded-2xl border border-gray-200 dark:border-slate-700 p-6 mb-6 shadow-sm">
            <h3 className="text-base font-semibold text-gray-900 dark:text-white mb-4">
              {editingId === undefined ? t.addCourse : t.editCourse}
            </h3>
            <form onSubmit={handleSubmit} className="space-y-4">
              <div>
                <label className="block text-sm font-medium text-gray-700 dark:text-slate-300 mb-1">
                  {t.courseName}
                </label>
                <input
                  type="text"
                  required
                  value={form.eventName}
                  onChange={e => setForm(f => ({ ...f, eventName: e.target.value }))}
                  className="w-full px-3 py-2 border border-gray-300 dark:border-slate-600 rounded-xl text-sm bg-white dark:bg-slate-700 text-gray-900 dark:text-white focus:ring-2 focus:ring-blue-500 focus:border-blue-500 outline-none transition-all"
                />
              </div>
              {editingId === undefined && (
                <div>
                  <label className="block text-sm font-medium text-gray-700 dark:text-slate-300 mb-1">
                    {t.semester}
                  </label>
                  <select
                    required
                    value={form.semesterId}
                    onChange={e => setForm(f => ({ ...f, semesterId: e.target.value }))}
                    className="w-full px-3 py-2 border border-gray-300 dark:border-slate-600 rounded-xl text-sm bg-white dark:bg-slate-700 text-gray-900 dark:text-white focus:ring-2 focus:ring-blue-500 focus:border-blue-500 outline-none transition-all"
                  >
                    <option value="">{t.selectSemester}</option>
                    {semesters.map(s => (
                      <option key={s.id} value={s.id}>{s.name}</option>
                    ))}
                  </select>
                </div>
              )}
              <div className="flex items-center gap-3 pt-2">
                <button
                  type="submit"
                  disabled={saving}
                  className="px-5 py-2 bg-blue-600 text-white rounded-xl text-sm font-semibold hover:bg-blue-700 transition-colors disabled:opacity-50"
                >
                  {saving ? t.saving : (editingId === undefined ? t.add : t.saveChanges)}
                </button>
                <button
                  type="button"
                  onClick={closeForm}
                  className="px-5 py-2 text-sm font-medium text-gray-600 dark:text-slate-300 border border-gray-300 dark:border-slate-600 rounded-xl hover:bg-gray-50 dark:hover:bg-slate-700 transition-colors"
                >
                  {t.cancel}
                </button>
              </div>
            </form>
          </div>
        )}

        {/* Content */}
        {loading ? (
          <div className="space-y-3">
            {[...Array(3)].map((_, i) => (
              <div key={i} className="bg-white dark:bg-slate-800 rounded-2xl border border-gray-200 dark:border-slate-700 p-5 animate-pulse">
                <div className="h-4 bg-gray-200 dark:bg-slate-700 rounded w-1/3 mb-2" />
                <div className="h-3 bg-gray-100 dark:bg-slate-600 rounded w-1/4" />
              </div>
            ))}
          </div>
        ) : error ? (
          <div className="bg-red-50 dark:bg-red-900/20 border border-red-200 dark:border-red-800 rounded-2xl p-10 text-center">
            <div className="text-4xl mb-3">⚠️</div>
            <p className="text-red-700 dark:text-red-400 font-semibold mb-1">{t.connectionError}</p>
            <p className="text-red-500 dark:text-red-400 text-sm mb-5">{error}</p>
            <button onClick={loadAll} className="px-5 py-2 text-sm font-medium text-red-600 dark:text-red-400 border border-red-300 dark:border-red-700 rounded-xl hover:bg-red-50 dark:hover:bg-red-900/30 transition-colors">
              {t.tryAgain}
            </button>
          </div>
        ) : filtered.length === 0 ? (
          <div className="flex flex-col items-center py-20 text-center">
            <div className="w-20 h-20 rounded-3xl bg-blue-50 dark:bg-blue-900/20 flex items-center justify-center text-4xl mb-5 shadow-inner">📚</div>
            <h2 className="text-xl font-semibold text-gray-800 dark:text-white">{t.noCourses}</h2>
            <p className="text-gray-500 dark:text-slate-400 mt-2 mb-7 text-sm max-w-xs">{t.noCoursesDesc}</p>
            <button onClick={openAdd} className="inline-flex items-center gap-2 px-5 py-2.5 bg-blue-600 text-white rounded-xl font-semibold text-sm hover:bg-blue-700 transition-colors shadow-sm">
              {t.addCourse}
            </button>
          </div>
        ) : (
          <div className="space-y-3">
            {filtered.map(c => {
              const sem = semesters.find(s => s.id === c.semesterId);
              return (
                <div
                  key={c.id}
                  className="bg-white dark:bg-slate-800 rounded-2xl border border-gray-200 dark:border-slate-700 shadow-sm hover:shadow-md transition-all duration-200 p-5 flex items-center justify-between gap-4"
                >
                  <div>
                    <h3 className="text-base font-semibold text-gray-900 dark:text-white">{c.eventName}</h3>
                    {sem && (
                      <p className="text-sm text-gray-500 dark:text-slate-400 mt-0.5">
                        🗓 {sem.name}
                      </p>
                    )}
                  </div>
                  <div className="flex items-center gap-2 flex-shrink-0">
                    <button
                      onClick={() => openEdit(c)}
                      className="px-3 py-1.5 text-xs font-semibold text-blue-700 dark:text-blue-300 bg-blue-50 dark:bg-blue-900/20 rounded-xl hover:bg-blue-100 dark:hover:bg-blue-900/40 transition-colors"
                    >
                      {t.edit}
                    </button>
                    <button
                      onClick={() => handleDelete(c.id)}
                      disabled={deletingId === c.id}
                      className="px-3 py-1.5 text-xs font-semibold text-red-600 dark:text-red-400 bg-red-50 dark:bg-red-900/20 rounded-xl hover:bg-red-100 dark:hover:bg-red-900/40 transition-colors disabled:opacity-50"
                    >
                      {deletingId === c.id ? '…' : t.delete}
                    </button>
                  </div>
                </div>
              );
            })}
          </div>
        )}
      </div>
    </MainLayout>
  );
}
