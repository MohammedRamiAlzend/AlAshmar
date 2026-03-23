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

interface MenuState {
  x: number;
  y: number;
  teacher: TeacherDto;
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
  const [menu, setMenu] = useState<MenuState | null>(null);

  const [page, setPage] = useState(1);
  const [pageSize] = useState(10);
  const [hasNextPage, setHasNextPage] = useState(false);

  const [editingId, setEditingId] = useState<string | null | undefined>(null);
  const [form, setForm] = useState<TeacherFormState>(emptyForm());

  useEffect(() => {
    void load(page);
  }, [page, pageSize]);

  useEffect(() => {
    const closeMenu = () => setMenu(null);
    window.addEventListener('click', closeMenu);
    return () => window.removeEventListener('click', closeMenu);
  }, []);

  const load = async (targetPage: number) => {
    try {
      setLoading(true);
      setError(null);
      const data = await teacherApi.listPaged(targetPage, pageSize);
      setTeachers(data);
      setHasNextPage(data.length === pageSize);

      if (targetPage > 1 && data.length === 0) {
        setPage(targetPage - 1);
      }
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
      await load(page);
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
      await load(page);
    } catch {
      alert(t.deleteError);
    } finally {
      setDeletingId(null);
    }
  };

  const handleResetPassword = async (teacher: TeacherDto) => {
    const password = prompt(`New password for ${teacher.name}:`);
    if (!password?.trim()) return;

    try {
      await teacherApi.resetPassword(teacher.id, password.trim());
      alert('Password reset successfully.');
    } catch {
      alert(t.connectionError);
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
      <div className="max-w-6xl mx-auto px-4 py-8">
        <div className="mb-6">
          <h2 className="text-2xl font-bold text-gray-900 dark:text-white">{t.teacherManagement}</h2>
          {!loading && !error && (
            <p className="text-sm text-gray-500 dark:text-slate-400 mt-0.5">
              Page {page} • {teachers.length} {t.teachers.toLowerCase()}
            </p>
          )}
        </div>

        {isFormOpen && (
          <div className="bg-white dark:bg-slate-800 rounded-2xl border border-gray-200 dark:border-slate-700 p-6 mb-6 shadow-sm">
            <h3 className="text-base font-semibold text-gray-900 dark:text-white mb-4">{editingId === undefined ? t.addTeacher : t.editTeacher}</h3>
            <form onSubmit={handleSubmit} className="space-y-4">
              <div className="grid sm:grid-cols-2 gap-4">
                <input type="text" required value={form.name} onChange={e => setForm(f => ({ ...f, name: e.target.value }))} placeholder={t.fullName} className="w-full px-3 py-2 border border-gray-300 dark:border-slate-600 rounded-xl text-sm bg-white dark:bg-slate-700 text-gray-900 dark:text-white" />
                <input type="email" value={form.email} onChange={e => setForm(f => ({ ...f, email: e.target.value }))} placeholder={t.email} className="w-full px-3 py-2 border border-gray-300 dark:border-slate-600 rounded-xl text-sm bg-white dark:bg-slate-700 text-gray-900 dark:text-white" />
                <input type="text" required value={form.fatherName} onChange={e => setForm(f => ({ ...f, fatherName: e.target.value }))} placeholder="Father Name" className="w-full px-3 py-2 border border-gray-300 dark:border-slate-600 rounded-xl text-sm bg-white dark:bg-slate-700 text-gray-900 dark:text-white" />
                <input type="text" required value={form.motherName} onChange={e => setForm(f => ({ ...f, motherName: e.target.value }))} placeholder="Mother Name" className="w-full px-3 py-2 border border-gray-300 dark:border-slate-600 rounded-xl text-sm bg-white dark:bg-slate-700 text-gray-900 dark:text-white" />
                <input type="text" required value={form.nationalityNumber} onChange={e => setForm(f => ({ ...f, nationalityNumber: e.target.value }))} placeholder="Nationality Number" className="w-full px-3 py-2 border border-gray-300 dark:border-slate-600 rounded-xl text-sm bg-white dark:bg-slate-700 text-gray-900 dark:text-white" />
              </div>

              {editingId === undefined && (
                <div className="space-y-3 pt-2">
                  <div className="flex items-center justify-between">
                    <h4 className="text-sm font-semibold text-gray-800 dark:text-slate-200">Contact Infos</h4>
                    <button type="button" onClick={addContactInfo} className="px-3 py-1.5 text-xs font-semibold text-blue-700 dark:text-blue-300 bg-blue-50 dark:bg-blue-900/20 rounded-xl">+ Add Contact</button>
                  </div>
                  {form.contactInfos.map((contact, index) => (
                    <div key={index} className="grid sm:grid-cols-[1fr_1fr_auto] gap-3 items-end">
                      <input type="text" required={index === 0} value={contact.number ?? ''} onChange={e => updateContactInfo(index, 'number', e.target.value)} placeholder={t.phone} className="w-full px-3 py-2 border border-gray-300 dark:border-slate-600 rounded-xl text-sm bg-white dark:bg-slate-700 text-gray-900 dark:text-white" />
                      <input type="email" value={contact.email ?? ''} onChange={e => updateContactInfo(index, 'email', e.target.value)} placeholder={t.email} className="w-full px-3 py-2 border border-gray-300 dark:border-slate-600 rounded-xl text-sm bg-white dark:bg-slate-700 text-gray-900 dark:text-white" />
                      <button type="button" disabled={form.contactInfos.length === 1} onClick={() => removeContactInfo(index)} className="px-3 py-2 text-xs font-semibold text-red-600 dark:text-red-400 bg-red-50 dark:bg-red-900/20 rounded-xl disabled:opacity-50">{t.delete}</button>
                    </div>
                  ))}
                </div>
              )}

              <div className="flex items-center gap-3 pt-2">
                <button type="submit" disabled={saving} className="px-5 py-2 bg-blue-600 text-white rounded-xl text-sm font-semibold disabled:opacity-50">{saving ? t.saving : (editingId === undefined ? t.add : t.saveChanges)}</button>
                <button type="button" onClick={closeForm} className="px-5 py-2 text-sm font-medium text-gray-600 dark:text-slate-300 border border-gray-300 dark:border-slate-600 rounded-xl">{t.cancel}</button>
              </div>
            </form>
          </div>
        )}

        {loading ? (
          <div className="text-center py-10 text-gray-500">Loading...</div>
        ) : error ? (
          <div className="bg-red-50 dark:bg-red-900/20 border border-red-200 dark:border-red-800 rounded-2xl p-10 text-center">
            <p className="text-red-700 dark:text-red-400 font-semibold mb-1">{t.connectionError}</p>
            <p className="text-red-500 dark:text-red-400 text-sm mb-5">{error}</p>
            <button onClick={() => load(page)} className="px-5 py-2 text-sm font-medium text-red-600 dark:text-red-400 border border-red-300 dark:border-red-700 rounded-xl">{t.tryAgain}</button>
          </div>
        ) : (
          <>
            <div className="overflow-x-auto bg-white dark:bg-slate-800 rounded-2xl border border-gray-200 dark:border-slate-700 shadow-sm">
              <table className="min-w-full text-sm">
                <thead className="bg-gray-50 dark:bg-slate-900/50">
                  <tr>
                    <th className="text-left px-4 py-3">Name</th>
                    <th className="text-left px-4 py-3">Father</th>
                    <th className="text-left px-4 py-3">Mother</th>
                    <th className="text-left px-4 py-3">Nationality</th>
                    <th className="text-left px-4 py-3">Email</th>
                    <th className="text-left px-4 py-3">Phone</th>
                    <th className="text-left px-4 py-3">Actions</th>
                  </tr>
                </thead>
                <tbody>
                  {teachers.map(teacher => (
                    <tr
                      key={teacher.id}
                      onContextMenu={e => {
                        e.preventDefault();
                        setMenu({ x: e.clientX, y: e.clientY, teacher });
                      }}
                      className="border-t border-gray-100 dark:border-slate-700 hover:bg-gray-50 dark:hover:bg-slate-700/40"
                    >
                      <td className="px-4 py-3">{teacher.name}</td>
                      <td className="px-4 py-3">{teacher.fatherName}</td>
                      <td className="px-4 py-3">{teacher.motherName}</td>
                      <td className="px-4 py-3">{teacher.nationalityNumber}</td>
                      <td className="px-4 py-3">{teacher.email || '-'}</td>
                      <td className="px-4 py-3">{teacher.contactInfos?.[0]?.number || '-'}</td>
                      <td className="px-4 py-3">
                        <div className="flex gap-2">
                          <button onClick={() => openEdit(teacher)} className="px-3 py-1.5 text-xs font-semibold text-blue-700 dark:text-blue-300 bg-blue-50 dark:bg-blue-900/20 rounded-xl">{t.edit}</button>
                          <button onClick={() => handleResetPassword(teacher)} className="px-3 py-1.5 text-xs font-semibold text-amber-700 dark:text-amber-300 bg-amber-50 dark:bg-amber-900/20 rounded-xl">Reset Password</button>
                          <button onClick={() => handleDelete(teacher.id)} disabled={deletingId === teacher.id} className="px-3 py-1.5 text-xs font-semibold text-red-600 dark:text-red-400 bg-red-50 dark:bg-red-900/20 rounded-xl disabled:opacity-50">{deletingId === teacher.id ? '…' : t.delete}</button>
                        </div>
                      </td>
                    </tr>
                  ))}
                </tbody>
              </table>
            </div>

            <div className="flex items-center justify-between mt-4">
              <button onClick={() => setPage(p => Math.max(1, p - 1))} disabled={page === 1} className="px-4 py-2 text-sm border rounded-xl disabled:opacity-50">Previous</button>
              <span className="text-sm text-gray-600 dark:text-slate-300">Page {page}</span>
              <button onClick={() => setPage(p => p + 1)} disabled={!hasNextPage} className="px-4 py-2 text-sm border rounded-xl disabled:opacity-50">Next</button>
            </div>
          </>
        )}
      </div>

      {menu && (
        <div
          className="fixed z-50 bg-white dark:bg-slate-800 border border-gray-200 dark:border-slate-700 rounded-xl shadow-lg py-2 min-w-44"
          style={{ top: menu.y, left: menu.x }}
          onClick={e => e.stopPropagation()}
        >
          <button className="w-full text-left px-4 py-2 text-sm hover:bg-gray-50 dark:hover:bg-slate-700" onClick={() => { openEdit(menu.teacher); setMenu(null); }}>Edit Info</button>
          <button className="w-full text-left px-4 py-2 text-sm hover:bg-gray-50 dark:hover:bg-slate-700" onClick={() => { void handleResetPassword(menu.teacher); setMenu(null); }}>Reset Password</button>
          <button className="w-full text-left px-4 py-2 text-sm hover:bg-gray-50 dark:hover:bg-slate-700" onClick={() => { void navigator.clipboard.writeText(menu.teacher.id); setMenu(null); }}>Copy ID</button>
          <button className="w-full text-left px-4 py-2 text-sm text-red-600 hover:bg-red-50 dark:hover:bg-red-900/20" onClick={() => { void handleDelete(menu.teacher.id); setMenu(null); }}>Delete</button>
        </div>
      )}
    </MainLayout>
  );
}
