import { useEffect, useState } from 'react';
import { courseApi, enrollmentApi, halaqaApi, studentApi } from '../api/academicApi';
import type { CourseDto, EnrollmentDto, HalaqaDto, StudentDto } from '../types/academic';
import MainLayout from '../components/MainLayout';
import { useT } from '../i18n';

interface EnrollmentFormState {
  studentId: string;
  halaqaId: string;
}

const emptyForm = (): EnrollmentFormState => ({ studentId: '', halaqaId: '' });

export default function EnrollmentPage() {
  const t = useT();
  const [students, setStudents] = useState<StudentDto[]>([]);
  const [courses, setCourses] = useState<CourseDto[]>([]);
  const [halaqas, setHalaqas] = useState<HalaqaDto[]>([]);
  const [enrollments, setEnrollments] = useState<EnrollmentDto[]>([]);

  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);
  const [saving, setSaving] = useState(false);
  const [deletingId, setDeletingId] = useState<string | null>(null);

  const [filterStudentId, setFilterStudentId] = useState('');
  const [filterCourseId, setFilterCourseId] = useState('');
  const [filterHalaqaId, setFilterHalaqaId] = useState('');

  const [form, setForm] = useState<EnrollmentFormState>(emptyForm());

  useEffect(() => {
    void loadAll();
  }, []);

  const loadAll = async () => {
    try {
      setLoading(true);
      setError(null);
      const [std, crs, hal] = await Promise.all([
        studentApi.list(),
        courseApi.list(),
        halaqaApi.list(),
      ]);

      const enr = await enrollmentApi.listByStudents(std.map(student => student.id));

      setStudents(std);
      setCourses(crs);
      setHalaqas(hal);
      setEnrollments(enr);

      setForm({
        studentId: std[0]?.id ?? '',
        halaqaId: hal[0]?.id ?? '',
      });
    } catch {
      setError(t.loadError);
    } finally {
      setLoading(false);
    }
  };

  const handleAssign = async (e: React.FormEvent) => {
    e.preventDefault();
    if (!form.studentId || !form.halaqaId) return;

    const duplicate = enrollments.some(enrollment =>
      enrollment.studentId === form.studentId && enrollment.halaqaId === form.halaqaId
    );

    if (duplicate) {
      alert(t.enrollmentExists);
      return;
    }

    setSaving(true);
    try {
      await enrollmentApi.create(form);
      const latest = await enrollmentApi.listByStudents(students.map(student => student.id));
      setEnrollments(latest);
    } catch {
      alert(t.connectionError);
    } finally {
      setSaving(false);
    }
  };

  const handleUnassign = async (id: string) => {
    if (!confirm(t.confirmDeleteEnrollment)) return;
    setDeletingId(id);
    try {
      await enrollmentApi.delete(id);
    } catch {
      alert('Enrollment delete is not available yet in backend API.');
    } finally {
      setDeletingId(null);
    }
  };

  const visibleHalaqas = filterCourseId
    ? halaqas.filter(halaqa => halaqa.courseId === filterCourseId)
    : halaqas;

  const filtered = enrollments.filter(enrollment => {
    const byStudent = !filterStudentId || enrollment.studentId === filterStudentId;
    const byHalaqa = !filterHalaqaId || enrollment.halaqaId === filterHalaqaId;
    const byCourse = !filterCourseId || halaqas.find(h => h.id === enrollment.halaqaId)?.courseId === filterCourseId;
    return byStudent && byHalaqa && byCourse;
  });

  return (
    <MainLayout>
      <div className="max-w-5xl mx-auto px-4 py-8">
        <div className="mb-6">
          <h2 className="text-2xl font-bold text-gray-900 dark:text-white">{t.enrollmentManagement}</h2>
          {!loading && !error && (
            <p className="text-sm text-gray-500 dark:text-slate-400 mt-0.5">
              {filtered.length} {t.enrollments.toLowerCase()}
            </p>
          )}
        </div>

        {!loading && !error && students.length > 0 && halaqas.length > 0 && (
          <div className="bg-white dark:bg-slate-800 rounded-2xl border border-gray-200 dark:border-slate-700 p-6 mb-6 shadow-sm">
            <h3 className="text-base font-semibold text-gray-900 dark:text-white mb-4">{t.assignStudentToHalaqa}</h3>
            <form onSubmit={handleAssign} className="grid lg:grid-cols-[1fr_1fr_auto] gap-3 items-end">
              <div>
                <label className="block text-sm font-medium text-gray-700 dark:text-slate-300 mb-1">{t.student}</label>
                <select
                  required
                  value={form.studentId}
                  onChange={e => setForm(prev => ({ ...prev, studentId: e.target.value }))}
                  className="w-full px-3 py-2 border border-gray-300 dark:border-slate-600 rounded-xl text-sm bg-white dark:bg-slate-700 text-gray-900 dark:text-white focus:ring-2 focus:ring-blue-500 focus:border-blue-500 outline-none transition-all"
                >
                  <option value="">{t.selectStudent}</option>
                  {students.map(student => (
                    <option key={student.id} value={student.id}>{student.name}</option>
                  ))}
                </select>
              </div>
              <div>
                <label className="block text-sm font-medium text-gray-700 dark:text-slate-300 mb-1">{t.halaqa}</label>
                <select
                  required
                  value={form.halaqaId}
                  onChange={e => setForm(prev => ({ ...prev, halaqaId: e.target.value }))}
                  className="w-full px-3 py-2 border border-gray-300 dark:border-slate-600 rounded-xl text-sm bg-white dark:bg-slate-700 text-gray-900 dark:text-white focus:ring-2 focus:ring-blue-500 focus:border-blue-500 outline-none transition-all"
                >
                  <option value="">{t.selectHalaqa}</option>
                  {halaqas.map(halaqa => (
                    <option key={halaqa.id} value={halaqa.id}>{halaqa.className}</option>
                  ))}
                </select>
              </div>
              <button
                type="submit"
                disabled={saving}
                className="px-5 py-2 bg-blue-600 text-white rounded-xl text-sm font-semibold hover:bg-blue-700 transition-colors disabled:opacity-50"
              >
                {saving ? t.saving : t.assign}
              </button>
            </form>
          </div>
        )}

        {!loading && !error && (
          <div className="bg-white dark:bg-slate-800 rounded-2xl border border-gray-200 dark:border-slate-700 p-6 mb-6 shadow-sm">
            <h3 className="text-base font-semibold text-gray-900 dark:text-white mb-4">{t.filters}</h3>
            <div className="grid sm:grid-cols-3 gap-3">
              <div>
                <label className="block text-sm font-medium text-gray-700 dark:text-slate-300 mb-1">{t.student}</label>
                <select
                  value={filterStudentId}
                  onChange={e => setFilterStudentId(e.target.value)}
                  className="w-full px-3 py-2 border border-gray-300 dark:border-slate-600 rounded-xl text-sm bg-white dark:bg-slate-700 text-gray-900 dark:text-white focus:ring-2 focus:ring-blue-500 focus:border-blue-500 outline-none transition-all"
                >
                  <option value="">{t.allStudents}</option>
                  {students.map(student => (
                    <option key={student.id} value={student.id}>{student.name}</option>
                  ))}
                </select>
              </div>
              <div>
                <label className="block text-sm font-medium text-gray-700 dark:text-slate-300 mb-1">{t.course}</label>
                <select
                  value={filterCourseId}
                  onChange={e => {
                    const nextCourseId = e.target.value;
                    setFilterCourseId(nextCourseId);
                    if (nextCourseId && filterHalaqaId) {
                      const halaqa = halaqas.find(item => item.id === filterHalaqaId);
                      if (halaqa && halaqa.courseId !== nextCourseId) {
                        setFilterHalaqaId('');
                      }
                    }
                  }}
                  className="w-full px-3 py-2 border border-gray-300 dark:border-slate-600 rounded-xl text-sm bg-white dark:bg-slate-700 text-gray-900 dark:text-white focus:ring-2 focus:ring-blue-500 focus:border-blue-500 outline-none transition-all"
                >
                  <option value="">{t.allCourses}</option>
                  {courses.map(course => (
                    <option key={course.id} value={course.id}>{course.eventName}</option>
                  ))}
                </select>
              </div>
              <div>
                <label className="block text-sm font-medium text-gray-700 dark:text-slate-300 mb-1">{t.halaqa}</label>
                <select
                  value={filterHalaqaId}
                  onChange={e => setFilterHalaqaId(e.target.value)}
                  className="w-full px-3 py-2 border border-gray-300 dark:border-slate-600 rounded-xl text-sm bg-white dark:bg-slate-700 text-gray-900 dark:text-white focus:ring-2 focus:ring-blue-500 focus:border-blue-500 outline-none transition-all"
                >
                  <option value="">{t.allHalaqas}</option>
                  {visibleHalaqas.map(halaqa => (
                    <option key={halaqa.id} value={halaqa.id}>{halaqa.className}</option>
                  ))}
                </select>
              </div>
            </div>
          </div>
        )}

        {loading ? (
          <div className="space-y-3">
            {[...Array(4)].map((_, i) => (
              <div key={i} className="bg-white dark:bg-slate-800 rounded-2xl border border-gray-200 dark:border-slate-700 p-5 animate-pulse">
                <div className="h-4 bg-gray-200 dark:bg-slate-700 rounded w-1/3 mb-2" />
                <div className="h-3 bg-gray-100 dark:bg-slate-600 rounded w-1/2" />
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
        ) : students.length === 0 ? (
          <div className="flex flex-col items-center py-20 text-center">
            <div className="w-20 h-20 rounded-3xl bg-blue-50 dark:bg-blue-900/20 flex items-center justify-center text-4xl mb-5 shadow-inner">👨‍🎓</div>
            <h2 className="text-xl font-semibold text-gray-800 dark:text-white">{t.noStudents}</h2>
            <p className="text-gray-500 dark:text-slate-400 mt-2 mb-7 text-sm max-w-xs">{t.enrollmentRequiresStudents}</p>
          </div>
        ) : halaqas.length === 0 ? (
          <div className="flex flex-col items-center py-20 text-center">
            <div className="w-20 h-20 rounded-3xl bg-blue-50 dark:bg-blue-900/20 flex items-center justify-center text-4xl mb-5 shadow-inner">🕌</div>
            <h2 className="text-xl font-semibold text-gray-800 dark:text-white">{t.noHalaqas}</h2>
            <p className="text-gray-500 dark:text-slate-400 mt-2 mb-7 text-sm max-w-xs">{t.enrollmentRequiresHalaqas}</p>
          </div>
        ) : filtered.length === 0 ? (
          <div className="flex flex-col items-center py-20 text-center">
            <div className="w-20 h-20 rounded-3xl bg-blue-50 dark:bg-blue-900/20 flex items-center justify-center text-4xl mb-5 shadow-inner">🔗</div>
            <h2 className="text-xl font-semibold text-gray-800 dark:text-white">{t.noEnrollments}</h2>
            <p className="text-gray-500 dark:text-slate-400 mt-2 mb-7 text-sm max-w-xs">{t.noEnrollmentsDesc}</p>
          </div>
        ) : (
          <div className="space-y-3">
            {filtered.map(enrollment => {
              const student = students.find(item => item.id === enrollment.studentId);
              const halaqa = halaqas.find(item => item.id === enrollment.halaqaId);
              const course = halaqa ? courses.find(item => item.id === halaqa.courseId) : null;

              return (
                <div
                  key={enrollment.id}
                  className="bg-white dark:bg-slate-800 rounded-2xl border border-gray-200 dark:border-slate-700 shadow-sm hover:shadow-md transition-all duration-200 p-5 flex items-center justify-between gap-4"
                >
                  <div>
                    <h3 className="text-base font-semibold text-gray-900 dark:text-white">
                      {student?.name ?? enrollment.student?.name ?? t.unknownStudent}
                    </h3>
                    <p className="text-sm text-gray-500 dark:text-slate-400 mt-0.5">
                      🕌 {halaqa?.className ?? enrollment.halaqa?.className ?? t.unknownHalaqa}
                    </p>
                    {course && (
                      <p className="text-xs text-gray-400 dark:text-slate-500 mt-1">📚 {course.eventName}</p>
                    )}
                  </div>
                  <button
                    onClick={() => handleUnassign(enrollment.id)}
                    disabled={deletingId === enrollment.id}
                    className="px-3 py-1.5 text-xs font-semibold text-red-600 dark:text-red-400 bg-red-50 dark:bg-red-900/20 rounded-xl hover:bg-red-100 dark:hover:bg-red-900/40 transition-colors disabled:opacity-50"
                  >
                    {deletingId === enrollment.id ? '…' : t.unassign}
                  </button>
                </div>
              );
            })}
          </div>
        )}
      </div>
    </MainLayout>
  );
}
