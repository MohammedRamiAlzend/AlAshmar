import { useState, type FC, type ReactNode } from 'react';
import AppHeader from './AppHeader';
import Sidebar from './Sidebar';

interface MainLayoutProps {
  children: ReactNode;
  headerContent?: ReactNode;
}

const MainLayout: FC<MainLayoutProps> = ({ children, headerContent }) => {
  const [mobileOpen, setMobileOpen] = useState(false);

  return (
    <div className="min-h-screen bg-gray-50 dark:bg-slate-900 transition-colors duration-300 flex flex-col">
      <AppHeader>
        <div className="flex items-center gap-2">
          {/* Mobile hamburger */}
          <button
            onClick={() => setMobileOpen(true)}
            className="lg:hidden p-2 rounded-xl text-gray-500 dark:text-slate-400 hover:bg-gray-100 dark:hover:bg-slate-700 transition-colors"
            aria-label="Open menu"
          >
            ☰
          </button>
          {headerContent}
        </div>
      </AppHeader>

      <div className="flex flex-1 overflow-hidden">
        <Sidebar mobileOpen={mobileOpen} onMobileClose={() => setMobileOpen(false)} />
        <main className="flex-1 overflow-y-auto">
          {children}
        </main>
      </div>
    </div>
  );
};

export default MainLayout;
