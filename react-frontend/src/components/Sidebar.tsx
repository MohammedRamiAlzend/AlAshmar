import { useState } from 'react';
import { NavLink } from 'react-router-dom';
import { useT } from '../i18n';
import { useDir } from '../i18n';

interface SidebarProps {
  mobileOpen: boolean;
  onMobileClose: () => void;
}

export default function Sidebar({ mobileOpen, onMobileClose }: SidebarProps) {
  const t = useT();
  const dir = useDir();
  const [collapsed, setCollapsed] = useState(false);

  const navLinkClass = ({ isActive }: { isActive: boolean }) =>
    `flex items-center gap-3 px-3 py-2.5 rounded-xl text-sm font-medium transition-colors duration-150 ${
      isActive
        ? 'bg-blue-50 dark:bg-blue-900/30 text-blue-700 dark:text-blue-300'
        : 'text-gray-600 dark:text-slate-300 hover:bg-gray-100 dark:hover:bg-slate-700'
    }`;

  const sidebarContent = (
    <div className="flex flex-col h-full">
      {/* Collapse toggle (desktop only) */}
      <div className="hidden lg:flex items-center justify-end px-3 py-2 border-b border-gray-200 dark:border-slate-700">
        <button
          onClick={() => setCollapsed(c => !c)}
          className="p-1.5 rounded-lg text-gray-400 hover:text-gray-600 dark:hover:text-slate-200 hover:bg-gray-100 dark:hover:bg-slate-700 transition-colors"
          title={collapsed ? t.expandSidebar : t.collapseSidebar}
        >
          {dir === 'rtl'
            ? (collapsed ? '◀' : '▶')
            : (collapsed ? '▶' : '◀')}
        </button>
      </div>

      <nav className="flex-1 overflow-y-auto px-3 py-4 space-y-6">
        {/* Forms section */}
        <div>
          {!collapsed && (
            <p className="px-3 mb-1.5 text-xs font-semibold uppercase tracking-wider text-gray-400 dark:text-slate-500">
              {t.formsSection}
            </p>
          )}
          <ul className="space-y-0.5">
            <li>
              <NavLink to="/" end className={navLinkClass}>
                <span className="text-base flex-shrink-0">📋</span>
                {!collapsed && <span>{t.myForms}</span>}
              </NavLink>
            </li>
          </ul>
        </div>

        {/* Academic section */}
        <div>
          {!collapsed && (
            <p className="px-3 mb-1.5 text-xs font-semibold uppercase tracking-wider text-gray-400 dark:text-slate-500">
              {t.academicSection}
            </p>
          )}
          <ul className="space-y-0.5">
            <li>
              <NavLink to="/semesters" className={navLinkClass}>
                <span className="text-base flex-shrink-0">🗓</span>
                {!collapsed && <span>{t.semesters}</span>}
              </NavLink>
            </li>
            <li>
              <NavLink to="/courses" className={navLinkClass}>
                <span className="text-base flex-shrink-0">📚</span>
                {!collapsed && <span>{t.courses}</span>}
              </NavLink>
            </li>
            <li>
              <NavLink to="/halaqas" className={navLinkClass}>
                <span className="text-base flex-shrink-0">🕌</span>
                {!collapsed && <span>{t.halaqas}</span>}
              </NavLink>
            </li>
            <li>
              <NavLink to="/students" className={navLinkClass}>
                <span className="text-base flex-shrink-0">👨‍🎓</span>
                {!collapsed && <span>{t.students}</span>}
              </NavLink>
            </li>
            <li>
              <NavLink to="/teachers" className={navLinkClass}>
                <span className="text-base flex-shrink-0">👨‍🏫</span>
                {!collapsed && <span>{t.teachers}</span>}
              </NavLink>
            </li>
            <li>
              <NavLink to="/enrollments" className={navLinkClass}>
                <span className="text-base flex-shrink-0">🔗</span>
                {!collapsed && <span>{t.enrollments}</span>}
              </NavLink>
            </li>
          </ul>
        </div>
      </nav>
    </div>
  );

  const sidebarWidth = collapsed ? 'w-16' : 'w-56';

  return (
    <>
      {/* Mobile overlay */}
      {mobileOpen && (
        <div
          className="fixed inset-0 bg-black/40 z-40 lg:hidden"
          onClick={onMobileClose}
        />
      )}

      {/* Mobile drawer */}
      <aside
        className={`fixed top-0 bottom-0 z-50 lg:hidden w-64 bg-white dark:bg-slate-800 border-e border-gray-200 dark:border-slate-700 shadow-xl transition-transform duration-300 ${
          dir === 'rtl' ? 'right-0' : 'left-0'
        } ${mobileOpen ? 'translate-x-0' : (dir === 'rtl' ? 'translate-x-full' : '-translate-x-full')}`}
      >
        {/* Mobile header */}
        <div className="flex items-center justify-between px-4 py-3 border-b border-gray-200 dark:border-slate-700">
          <span className="text-sm font-semibold text-gray-700 dark:text-slate-200">{t.appName}</span>
          <button
            onClick={onMobileClose}
            className="p-1.5 rounded-lg text-gray-400 hover:text-gray-600 dark:hover:text-slate-200 hover:bg-gray-100 dark:hover:bg-slate-700 transition-colors"
          >
            ✕
          </button>
        </div>
        {sidebarContent}
      </aside>

      {/* Desktop sidebar */}
      <aside
        className={`hidden lg:flex flex-col ${sidebarWidth} flex-shrink-0 bg-white dark:bg-slate-800 border-e border-gray-200 dark:border-slate-700 transition-all duration-200 overflow-hidden`}
      >
        {sidebarContent}
      </aside>
    </>
  );
}
