import { useEffect, useState } from 'react';
import { teacherApi } from '../api/academicApi';
import type { CreateTeacherContactInfoDto, TeacherDto } from '../types/academic';
import MainLayout from '../components/MainLayout';
import { useT } from '../i18n';

interface TeacherFormState {
  name: string;
  fatherName: string;
  motherName: string;
  nationalityNumber: string;
  email: string;
  contactInfos: CreateTeacherContactInfoDto[];
}

const emptyForm = (): TeacherFormState => ({
  name: '',
  fatherName: '',
  motherName: '',
  nationalityNumber: '',
  email: '',
  contactInfos: [{ number: '', email: '', isActive: true }],
});

export default function TeacherListPage() {
  const t = useT();
  const [teachers, setTeachers] = useState<TeacherDto[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);
  const [saving, setSaving] = useState(false);
  const [deletingId, setDeletingId] = useState<string | null>(null);

  // null = closed, undefined = new, string = editing id
  const [editingId, setEditingId] = useState<string | null | undefined>(null);
  const [form, setForm] = useState<TeacherFormState>(emptyForm());

  useEffect(() => {
    load();
  }, []);

  const load = async () => {
    try {
      setLoading(true);
      setError(null);
      setTeachers(await teacherApi.list());
    } catch {
      setError(t.loadError);
    } finally {
      setLoading(false);
    }
  };

  const openAdd = () => {
    setForm(emptyForm());
    setEditingId(undefined);
  };

  const openEdit = (teacher: TeacherDto) => {
    setForm({
      name: teacher.name ?? '',
      email: teacher.email ?? '',
      fatherName: teacher.fatherName ?? '',
      motherName: teacher.motherName ?? '',
      nationalityNumber: teacher.nationalityNumber ?? '',
      contactInfos: [{ number: teacher.contactInfos?.[0]?.number ?? '', email: teacher.email ?? '', isActive: true }],
    });
    setEditingId(teacher.id);
  };

  const closeForm = () => setEditingId(null);

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setSaving(true);
    try {
      if (editingId === undefined) {
        await teacherApi.create({
          name: form.name,
          fatherName: form.fatherName,
          motherName: form.motherName,
          nationalityNumber: form.nationalityNumber,
          email: form.email || undefined,
          contactInfos: form.contactInfos.filter(c => c.number.trim().length > 0),
        });
      } else if (editingId) {
        await teacherApi.update(editingId, {
          name: form.name,
          fatherName: form.fatherName,
          motherName: form.motherName,
          nationalityNumber: form.nationalityNumber,
          email: form.email || undefined,
        });
      }
      await load();
      closeForm();
    } catch {
      alert(t.connectionError);
    } finally {
      setSaving(false);
    }
  };

  const handleDelete = async (id: string) => {
    if (!confirm(t.confirmDeleteTeacher)) return;
    setDeletingId(id);
    try {
      await teacherApi.delete(id);
      setTeachers(prev => prev.filter(teacher => teacher.id !== id));
    } catch {
      alert(t.deleteError);
    } finally {
      setDeletingId(null);
    }
  };

  const isFormOpen = editingId !== null;

  const addContactInfo = () => {
    setForm(prev => ({
      ...prev,
      contactInfos: [...prev.contactInfos, { number: '', email: prev.email || '', isActive: true }],
    }));
  };

  const removeContactInfo = (index: number) => {
    setForm(prev => ({
      ...prev,
      contactInfos: prev.contactInfos.filter((_, i) => i !== index),
    }));
  };

  const updateContactInfo = (index: number, field: keyof CreateTeacherContactInfoDto, value: string | boolean) => {
    setForm(prev => ({
      ...prev,
      contactInfos: prev.contactInfos.map((contact, i) => i === index ? { ...contact, [field]: value } : contact),
    }));
  };

  return (
    <MainLayout
      headerContent={
        <div className="flex justify-end w-full">
          <button
            onClick={openAdd}
            className="flex items-center gap-1.5 px-4 py-2 bg-blue-600 text-white rounded-xl font-semibold text-sm hover:bg-blue-700 active:bg-blue-800 transition-colors shadow-sm"
          >
            {t.addTeacher}
          </button>
        </div>
      }
    >
      <div className="max-w-4xl mx-auto px-4 py-8">
        <div className="mb-6">
          <h2 className="text-2xl font-bold text-gray-900 dark:text-white">{t.teacherManagement}</h2>
          {!loading && !error && (
            <p className="text-sm text-gray-500 dark:text-slate-400 mt-0.5">
              {teachers.length} {t.teachers.toLowerCase()}
            </p>
          )}
        </div>

        {isFormOpen && (
          <div className="bg-white dark:bg-slate-800 rounded-2xl border border-gray-200 dark:border-slate-700 p-6 mb-6 shadow-sm">
            <h3 className="text-base font-semibold text-gray-900 dark:text-white mb-4">
              {editingId === undefined ? t.addTeacher : t.editTeacher}
            </h3>
            <form onSubmit={handleSubmit} className="space-y-4">
              <div className="grid sm:grid-cols-2 gap-4">
                <div>
                  <label className="block text-sm font-medium text-gray-700 dark:text-slate-300 mb-1">
                    {t.fullName}
                  </label>
                  <input
                    type="text"
                    required
                    value={form.name}
                    onChange={e => setForm(f => ({ ...f, name: e.target.value }))}
                    className="w-full px-3 py-2 border border-gray-300 dark:border-slate-600 rounded-xl text-sm bg-white dark:bg-slate-700 text-gray-900 dark:text-white focus:ring-2 focus:ring-blue-500 focus:border-blue-500 outline-none transition-all"
                  />
                </div>
                <div>
                  <label className="block text-sm font-medium text-gray-700 dark:text-slate-300 mb-1">
                    {t.email}
                  </label>
                  <input
                    type="email"
                    value={form.email}
                    onChange={e => setForm(f => ({ ...f, email: e.target.value }))}
                    className="w-full px-3 py-2 border border-gray-300 dark:border-slate-600 rounded-xl text-sm bg-white dark:bg-slate-700 text-gray-900 dark:text-white focus:ring-2 focus:ring-blue-500 focus:border-blue-500 outline-none transition-all"
                  />
                </div>
                <div>
                  <label className="block text-sm font-medium text-gray-700 dark:text-slate-300 mb-1">
                    Father Name
                  </label>
                  <input
                    type="text"
                    required
                    value={form.fatherName}
                    onChange={e => setForm(f => ({ ...f, fatherName: e.target.value }))}
                    className="w-full px-3 py-2 border border-gray-300 dark:border-slate-600 rounded-xl text-sm bg-white dark:bg-slate-700 text-gray-900 dark:text-white focus:ring-2 focus:ring-blue-500 focus:border-blue-500 outline-none transition-all"
                  />
                </div>
                <div>
                  <label className="block text-sm font-medium text-gray-700 dark:text-slate-300 mb-1">
                    Mother Name
                  </label>
                  <input
                    type="text"
                    required
                    value={form.motherName}
                    onChange={e => setForm(f => ({ ...f, motherName: e.target.value }))}
                    className="w-full px-3 py-2 border border-gray-300 dark:border-slate-600 rounded-xl text-sm bg-white dark:bg-slate-700 text-gray-900 dark:text-white focus:ring-2 focus:ring-blue-500 focus:border-blue-500 outline-none transition-all"
                  />
                </div>
                <div>
                  <label className="block text-sm font-medium text-gray-700 dark:text-slate-300 mb-1">
                    Nationality Number
                  </label>
                  <input
                    type="text"
                    required
                    value={form.nationalityNumber}
                    onChange={e => setForm(f => ({ ...f, nationalityNumber: e.target.value }))}
                    className="w-full px-3 py-2 border border-gray-300 dark:border-slate-600 rounded-xl text-sm bg-white dark:bg-slate-700 text-gray-900 dark:text-white focus:ring-2 focus:ring-blue-500 focus:border-blue-500 outline-none transition-all"
                  />
                </div>
              </div>

              {editingId === undefined && (
                <div className="space-y-3 pt-2">
                  <div className="flex items-center justify-between">
                    <h4 className="text-sm font-semibold text-gray-800 dark:text-slate-200">Contact Infos</h4>
                    <button
                      type="button"
                      onClick={addContactInfo}
                      className="px-3 py-1.5 text-xs font-semibold text-blue-700 dark:text-blue-300 bg-blue-50 dark:bg-blue-900/20 rounded-xl hover:bg-blue-100 dark:hover:bg-blue-900/40 transition-colors"
                    >
                      + Add Contact
                    </button>
                  </div>

                  {form.contactInfos.map((contact, index) => (
                    <div key={index} className="grid sm:grid-cols-[1fr_1fr_auto] gap-3 items-end">
                      <div>
                        <label className="block text-sm font-medium text-gray-700 dark:text-slate-300 mb-1">{t.phone}</label>
                        <input
                          type="text"
                          required={index === 0}
                          value={contact.number ?? ''}
                          onChange={e => updateContactInfo(index, 'number', e.target.value)}
                          className="w-full px-3 py-2 border border-gray-300 dark:border-slate-600 rounded-xl text-sm bg-white dark:bg-slate-700 text-gray-900 dark:text-white focus:ring-2 focus:ring-blue-500 focus:border-blue-500 outline-none transition-all"
                        />
                      </div>
                      <div>
                        <label className="block text-sm font-medium text-gray-700 dark:text-slate-300 mb-1">{t.email}</label>
                        <input
                          type="email"
                          value={contact.email ?? ''}
                          onChange={e => updateContactInfo(index, 'email', e.target.value)}
                          className="w-full px-3 py-2 border border-gray-300 dark:border-slate-600 rounded-xl text-sm bg-white dark:bg-slate-700 text-gray-900 dark:text-white focus:ring-2 focus:ring-blue-500 focus:border-blue-500 outline-none transition-all"
                        />
                      </div>
                      <button
                        type="button"
                        disabled={form.contactInfos.length === 1}
                        onClick={() => removeContactInfo(index)}
                        className="px-3 py-2 text-xs font-semibold text-red-600 dark:text-red-400 bg-red-50 dark:bg-red-900/20 rounded-xl hover:bg-red-100 dark:hover:bg-red-900/40 transition-colors disabled:opacity-50"
                      >
                        {t.delete}
                      </button>
                    </div>
                  ))}
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

        {loading ? (
          <div className="space-y-3">
            {[...Array(3)].map((_, i) => (
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
            <button onClick={load} className="px-5 py-2 text-sm font-medium text-red-600 dark:text-red-400 border border-red-300 dark:border-red-700 rounded-xl hover:bg-red-50 dark:hover:bg-red-900/30 transition-colors">
              {t.tryAgain}
            </button>
          </div>
        ) : teachers.length === 0 ? (
          <div className="flex flex-col items-center py-20 text-center">
            <div className="w-20 h-20 rounded-3xl bg-blue-50 dark:bg-blue-900/20 flex items-center justify-center text-4xl mb-5 shadow-inner">👨‍🏫</div>
            <h2 className="text-xl font-semibold text-gray-800 dark:text-white">{t.noTeachers}</h2>
            <p className="text-gray-500 dark:text-slate-400 mt-2 mb-7 text-sm max-w-xs">{t.noTeachersDesc}</p>
            <button onClick={openAdd} className="inline-flex items-center gap-2 px-5 py-2.5 bg-blue-600 text-white rounded-xl font-semibold text-sm hover:bg-blue-700 transition-colors shadow-sm">
              {t.addTeacher}
            </button>
          </div>
        ) : (
          <div className="space-y-3">
            {teachers.map(teacher => (
              <div
                key={teacher.id}
                className="bg-white dark:bg-slate-800 rounded-2xl border border-gray-200 dark:border-slate-700 shadow-sm hover:shadow-md transition-all duration-200 p-5 flex items-center justify-between gap-4"
              >
                <div>
                  <h3 className="text-base font-semibold text-gray-900 dark:text-white">{teacher.name}</h3>
                  <p className="text-sm text-gray-500 dark:text-slate-400 mt-0.5">
                    {teacher.fatherName} • {teacher.motherName}
                  </p>
                  <p className="text-xs text-gray-400 dark:text-slate-500 mt-1">
                    Nationality Number: {teacher.nationalityNumber}
                  </p>
                  <p className="text-xs text-gray-400 dark:text-slate-500 mt-1">
                    {t.email}: {teacher.email || '-'} • {t.phone}: {teacher.contactInfos?.[0]?.number || '-'}
                  </p>
                </div>
                <div className="flex items-center gap-2 flex-shrink-0">
                  <button
                    onClick={() => openEdit(teacher)}
                    className="px-3 py-1.5 text-xs font-semibold text-blue-700 dark:text-blue-300 bg-blue-50 dark:bg-blue-900/20 rounded-xl hover:bg-blue-100 dark:hover:bg-blue-900/40 transition-colors"
                  >
                    {t.edit}
                  </button>
                  <button
                    onClick={() => handleDelete(teacher.id)}
                    disabled={deletingId === teacher.id}
                    className="px-3 py-1.5 text-xs font-semibold text-red-600 dark:text-red-400 bg-red-50 dark:bg-red-900/20 rounded-xl hover:bg-red-100 dark:hover:bg-red-900/40 transition-colors disabled:opacity-50"
                  >
                    {deletingId === teacher.id ? '…' : t.delete}
                  </button>
                </div>
              </div>
            ))}
          </div>
        )}
      </div>
    </MainLayout>
  );
}
