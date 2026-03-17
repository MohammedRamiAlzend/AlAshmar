import { BrowserRouter, Routes, Route } from 'react-router-dom';
import FormListPage from './pages/FormListPage';
import FormBuilderPage from './pages/FormBuilderPage';
import FormPreviewPage from './pages/FormPreviewPage';
import FormFillPage from './pages/FormFillPage';
import FormResponsesPage from './pages/FormResponsesPage';

export default function App() {
  return (
    <BrowserRouter>
      <Routes>
        <Route path="/" element={<FormListPage />} />
        <Route path="/forms/new" element={<FormBuilderPage />} />
        <Route path="/forms/:id/edit" element={<FormBuilderPage />} />
        <Route path="/forms/:id/preview" element={<FormPreviewPage />} />
        <Route path="/forms/:id/responses" element={<FormResponsesPage />} />
        <Route path="/fill/:accessToken" element={<FormFillPage />} />
      </Routes>
    </BrowserRouter>
  );
}
