import { useEffect, type FC, type ReactNode } from 'react';
import { BrowserRouter, Routes, Route, Navigate } from 'react-router-dom';
import { useAuthStore } from './store/authStore';
import { useSettingsStore } from './store/settingsStore';
import LoginPage from './pages/LoginPage';
import FormListPage from './pages/FormListPage';
import FormBuilderPage from './pages/FormBuilderPage';
import FormPreviewPage from './pages/FormPreviewPage';
import FormFillPage from './pages/FormFillPage';
import FormResponsesPage from './pages/FormResponsesPage';

function AppEffects() {
  const { theme, lang } = useSettingsStore();

  useEffect(() => {
    document.documentElement.classList.toggle('dark', theme === 'dark');
    document.documentElement.setAttribute('dir', lang === 'ar' ? 'rtl' : 'ltr');
    document.documentElement.setAttribute('lang', lang);
    if (lang === 'ar') {
      document.documentElement.style.fontFamily = "'Tajawal', 'Cairo', sans-serif";
    } else {
      document.documentElement.style.fontFamily = "'Inter', sans-serif";
    }
  }, [theme, lang]);

  return null;
}

const ProtectedRoute: FC<{ children: ReactNode }> = ({ children }) => {
  const isAuthenticated = useAuthStore(s => s.isAuthenticated);
  return isAuthenticated() ? <>{children}</> : <Navigate to="/login" replace />;
};

export default function App() {
  return (
    <BrowserRouter>
      <AppEffects />
      <Routes>
        <Route path="/login" element={<LoginPage />} />
        <Route path="/fill/:accessToken" element={<FormFillPage />} />
        <Route path="/" element={<ProtectedRoute><FormListPage /></ProtectedRoute>} />
        <Route path="/forms/new" element={<ProtectedRoute><FormBuilderPage /></ProtectedRoute>} />
        <Route path="/forms/:id/edit" element={<ProtectedRoute><FormBuilderPage /></ProtectedRoute>} />
        <Route path="/forms/:id/preview" element={<ProtectedRoute><FormPreviewPage /></ProtectedRoute>} />
        <Route path="/forms/:id/responses" element={<ProtectedRoute><FormResponsesPage /></ProtectedRoute>} />
      </Routes>
    </BrowserRouter>
  );
}
