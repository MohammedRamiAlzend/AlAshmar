import { useState, useEffect, useCallback } from 'react';
import { useParams, useNavigate, Link } from 'react-router-dom';
import {
  DndContext,
  closestCenter,
  KeyboardSensor,
  PointerSensor,
  useSensor,
  useSensors,
  type DragEndEvent,
} from '@dnd-kit/core';
import {
  arrayMove,
  SortableContext,
  sortableKeyboardCoordinates,
  useSortable,
  verticalListSortingStrategy,
} from '@dnd-kit/sortable';
import { CSS } from '@dnd-kit/utilities';
import { formApi, questionApi } from '../api/formApi';
import type { FormQuestionDto, QuestionType, FormType, AudienceType } from '../types/form';
import QuestionEditor from '../components/QuestionEditor';
import { FONT_FAMILIES_GROUPED } from '../config';
import AppHeader from '../components/AppHeader';
import { useT } from '../i18n';

function SortableQuestion({
  question,
  isQuiz,
  onChange,
  onDelete,
}: {
  question: FormQuestionDto;
  isQuiz: boolean;
  onChange: (q: FormQuestionDto) => void;
  onDelete: () => void;
}) {
  const { attributes, listeners, setNodeRef, transform, transition, isDragging } =
    useSortable({ id: question.id });

  const style: React.CSSProperties = {
    transform: CSS.Transform.toString(transform),
    transition,
    opacity: isDragging ? 0.5 : 1,
    width: `${Math.round((question.columnSpan || 12) / 12 * 100)}%`,
  };

  return (
    <div ref={setNodeRef} style={style}>
      <div className="flex items-start gap-2">
        <button
          {...attributes}
          {...listeners}
          className="mt-4 p-1 text-gray-400 hover:text-gray-600 cursor-grab active:cursor-grabbing"
          title="Drag to reorder"
        >
          ⠿
        </button>
        <div className="flex-1">
          <QuestionEditor
            question={question}
            isQuiz={isQuiz}
            onChange={onChange}
            onDelete={onDelete}
          />
        </div>
      </div>
    </div>
  );
}

const DEFAULT_QUESTION: Omit<FormQuestionDto, 'id' | 'formId'> = {
  text: '',
  questionType: 'ShortText',
  order: 0,
  isRequired: false,
  columnSpan: 12,
  options: [],
};

export default function FormBuilderPage() {
  const { id } = useParams<{ id: string }>();
  const navigate = useNavigate();
  const isEdit = Boolean(id);
  const t = useT();

  const [loading, setLoading] = useState(isEdit);
  const [saving, setSaving] = useState(false);
  const [error, setError] = useState<string | null>(null);
  const [successMsg, setSuccessMsg] = useState<string | null>(null);

  const [title, setTitle] = useState('Untitled Form');
  const [description, setDescription] = useState('');
  const [formType, setFormType] = useState<FormType>('Normal');
  const [audience, setAudience] = useState<AudienceType>('Students');
  const [isActive, setIsActive] = useState(true);
  const [allowMultipleResponses, setAllowMultipleResponses] = useState(false);
  const [timerMinutes, setTimerMinutes] = useState<number | undefined>(undefined);

  const [primaryColor, setPrimaryColor] = useState('#3b82f6');
  const [backgroundColor, setBackgroundColor] = useState('#f9fafb');
  const [fontFamily, setFontFamily] = useState('Inter');

  const [questions, setQuestions] = useState<FormQuestionDto[]>([]);
  const [showStylePanel, setShowStylePanel] = useState(false);

  const sensors = useSensors(
    useSensor(PointerSensor),
    useSensor(KeyboardSensor, { coordinateGetter: sortableKeyboardCoordinates })
  );

  useEffect(() => {
    if (isEdit && id) {
      formApi.get(id)
        .then(form => {
          setTitle(form.title);
          setDescription(form.description || '');
          setFormType(form.formType);
          setAudience(form.audience);
          setIsActive(form.isActive);
          setAllowMultipleResponses(form.allowMultipleResponses);
          setTimerMinutes(form.timerMinutes);
          setPrimaryColor(form.primaryColor || '#3b82f6');
          setBackgroundColor(form.backgroundColor || '#f9fafb');
          setFontFamily(form.fontFamily || 'Inter');
          const sorted = [...(form.questions || [])].sort((a, b) => a.order - b.order);
          setQuestions(sorted);
        })
        .catch(() => setError('Failed to load form'))
        .finally(() => setLoading(false));
    }
  }, [id, isEdit]);

  const handleDragEnd = useCallback((event: DragEndEvent) => {
    const { active, over } = event;
    if (!over || active.id === over.id) return;
    setQuestions(qs => {
      const oldIdx = qs.findIndex(q => q.id === active.id);
      const newIdx = qs.findIndex(q => q.id === over.id);
      return arrayMove(qs, oldIdx, newIdx).map((q, i) => ({ ...q, order: i }));
    });
  }, []);

  const addQuestion = (type: QuestionType = 'ShortText') => {
    const newQ: FormQuestionDto = {
      ...DEFAULT_QUESTION,
      id: `temp-${Date.now()}`,
      formId: id || '',
      text: '',
      questionType: type,
      order: questions.length,
      options: [],
    };
    setQuestions(qs => [...qs, newQ]);
  };

  const updateQuestion = (idx: number, updated: FormQuestionDto) => {
    setQuestions(qs => qs.map((q, i) => i === idx ? updated : q));
  };

  const deleteQuestion = (idx: number) => {
    setQuestions(qs => qs.filter((_, i) => i !== idx).map((q, i) => ({ ...q, order: i })));
  };

  const handleSave = async () => {
    if (!title.trim()) {
      setError('Form title is required');
      return;
    }
    setSaving(true);
    setError(null);
    setSuccessMsg(null);
    try {
      let formId = id;
      const formData = {
        title: title.trim(),
        description: description.trim() || undefined,
        formType,
        audience,
        isActive,
        allowMultipleResponses,
        timerMinutes: timerMinutes || undefined,
        primaryColor,
        backgroundColor,
        fontFamily,
      };

      if (isEdit && id) {
        await formApi.update(id, formData);
      } else {
        const created = await formApi.create(formData);
        formId = created.id;
      }

      if (formId) {
        for (let i = 0; i < questions.length; i++) {
          const q = { ...questions[i], order: i };
          if (q.id.startsWith('temp-')) {
            const created = await questionApi.create({
              formId,
              text: q.text || 'Question',
              description: q.description,
              questionType: q.questionType,
              order: q.order,
              isRequired: q.isRequired,
              points: q.points,
              columnSpan: q.columnSpan,
              labelColor: q.labelColor,
              fontSize: q.fontSize,
              fontFamily: q.fontFamily,
              options: q.options
                .filter(o => o.id.startsWith('temp-'))
                .map((o, oi) => ({ text: o.text, order: oi, isCorrect: o.isCorrect })),
            });
            setQuestions(prev => prev.map(pq => pq.id === q.id ? { ...created } : pq));
          } else {
            await questionApi.update(q.id, {
              text: q.text || 'Question',
              description: q.description,
              questionType: q.questionType,
              order: q.order,
              isRequired: q.isRequired,
              points: q.points,
              columnSpan: q.columnSpan,
              labelColor: q.labelColor,
              fontSize: q.fontSize,
              fontFamily: q.fontFamily,
            });
          }
        }
      }

      setSuccessMsg('Form saved successfully!');
      if (!isEdit && formId) {
        navigate(`/forms/${formId}/edit`);
      }
    } catch {
      setError('Failed to save form. Please try again.');
    } finally {
      setSaving(false);
    }
  };

  if (loading) {
    return (
      <div className="min-h-screen flex items-center justify-center bg-gray-50 dark:bg-slate-900">
        <div className="animate-spin w-8 h-8 border-4 border-blue-600 border-t-transparent rounded-full" />
      </div>
    );
  }

  return (
    <div className="min-h-screen dark:bg-slate-900 transition-colors duration-300" style={{ backgroundColor, fontFamily }}>
      {/* Top bar */}
      <AppHeader>
        <div className="flex items-center gap-3 ms-2">
          <Link to="/" className="text-gray-500 dark:text-slate-400 hover:text-gray-700 dark:hover:text-white text-sm">{t.back}</Link>
          <span className="text-gray-300 dark:text-slate-600">|</span>
          <input
            type="text"
            value={title}
            onChange={e => setTitle(e.target.value)}
            placeholder="Form title"
            className="text-base font-semibold text-gray-900 dark:text-white border-none outline-none bg-transparent w-48 sm:w-64"
          />
        </div>
      </AppHeader>

      {/* Save bar */}
      <div className="bg-white dark:bg-slate-800 border-b border-gray-200 dark:border-slate-700">
        <div className="max-w-5xl mx-auto px-4 py-2 flex items-center justify-end gap-3">
          {error && <span className="text-sm text-red-500">{error}</span>}
          {successMsg && <span className="text-sm text-green-600 dark:text-green-400">{successMsg}</span>}
          {isEdit && (
            <Link
              to={`/forms/${id}/preview`}
              className="px-4 py-2 text-sm text-gray-600 dark:text-slate-300 border border-gray-300 dark:border-slate-600 rounded-lg hover:bg-gray-50 dark:hover:bg-slate-700"
            >
              {t.preview}
            </Link>
          )}
          <button
            onClick={handleSave}
            disabled={saving}
            className="px-5 py-2 bg-blue-600 text-white text-sm font-semibold rounded-lg hover:bg-blue-700 disabled:opacity-50 transition-colors"
          >
            {saving ? t.saving : t.save}
          </button>
        </div>
      </div>

      <div className="max-w-5xl mx-auto px-4 py-8 space-y-6">
        {/* Form header card */}
        <div className="bg-white dark:bg-slate-800 rounded-xl border-t-8 shadow-sm p-6 space-y-4" style={{ borderTopColor: primaryColor }}>
          <textarea
            value={title}
            onChange={e => setTitle(e.target.value)}
            placeholder="Form Title"
            rows={1}
            className="w-full text-3xl font-bold text-gray-900 dark:text-white border-none outline-none resize-none bg-transparent"
          />
          <textarea
            value={description}
            onChange={e => setDescription(e.target.value)}
            placeholder="Form description (optional)"
            rows={2}
            className="w-full text-base text-gray-500 dark:text-slate-400 border-none outline-none resize-none bg-transparent"
          />

          {/* Form settings */}
          <div className="grid grid-cols-2 gap-4 pt-4 border-t border-gray-100 dark:border-slate-700">
            <div>
              <label className="text-xs font-medium text-gray-500 dark:text-slate-400 uppercase tracking-wide">Form Type</label>
              <select
                value={formType}
                onChange={e => setFormType(e.target.value as FormType)}
                className="mt-1 w-full px-3 py-2 border border-gray-300 dark:border-slate-600 rounded-lg text-sm bg-white dark:bg-slate-700 text-gray-900 dark:text-white focus:ring-2 focus:ring-blue-500 outline-none"
              >
                <option value="Normal">Normal</option>
                <option value="Quiz">Quiz</option>
              </select>
            </div>
            <div>
              <label className="text-xs font-medium text-gray-500 dark:text-slate-400 uppercase tracking-wide">Audience</label>
              <select
                value={audience}
                onChange={e => setAudience(e.target.value as AudienceType)}
                className="mt-1 w-full px-3 py-2 border border-gray-300 dark:border-slate-600 rounded-lg text-sm bg-white dark:bg-slate-700 text-gray-900 dark:text-white focus:ring-2 focus:ring-blue-500 outline-none"
              >
                <option value="Students">Students</option>
                <option value="Teachers">Teachers</option>
                <option value="Both">Both</option>
              </select>
            </div>
            <div>
              <label className="text-xs font-medium text-gray-500 dark:text-slate-400 uppercase tracking-wide">Timer (minutes)</label>
              <input
                type="number"
                value={timerMinutes || ''}
                onChange={e => setTimerMinutes(e.target.value ? parseInt(e.target.value) : undefined)}
                placeholder="No timer"
                min={1}
                className="mt-1 w-full px-3 py-2 border border-gray-300 dark:border-slate-600 rounded-lg text-sm bg-white dark:bg-slate-700 text-gray-900 dark:text-white focus:ring-2 focus:ring-blue-500 outline-none"
              />
            </div>
            <div className="flex flex-col gap-3 pt-4">
              <label className="flex items-center gap-3 cursor-pointer">
                <div
                  onClick={() => setIsActive(a => !a)}
                  className={`relative w-10 h-5 rounded-full transition-colors cursor-pointer ${isActive ? 'bg-blue-600' : 'bg-gray-300 dark:bg-slate-600'}`}
                >
                  <div className={`absolute top-0.5 w-4 h-4 bg-white rounded-full shadow transition-transform ${isActive ? 'translate-x-5' : 'translate-x-0.5'}`} />
                </div>
                <span className="text-sm text-gray-700 dark:text-slate-300">Active</span>
              </label>
              <label className="flex items-center gap-3 cursor-pointer">
                <div
                  onClick={() => setAllowMultipleResponses(a => !a)}
                  className={`relative w-10 h-5 rounded-full transition-colors cursor-pointer ${allowMultipleResponses ? 'bg-blue-600' : 'bg-gray-300 dark:bg-slate-600'}`}
                >
                  <div className={`absolute top-0.5 w-4 h-4 bg-white rounded-full shadow transition-transform ${allowMultipleResponses ? 'translate-x-5' : 'translate-x-0.5'}`} />
                </div>
                <span className="text-sm text-gray-700 dark:text-slate-300">Allow Multiple Responses</span>
              </label>
            </div>
          </div>

          {/* Style panel toggle */}
          <div className="pt-2 border-t border-gray-100 dark:border-slate-700">
            <button
              onClick={() => setShowStylePanel(s => !s)}
              className="text-sm text-gray-500 dark:text-slate-400 hover:text-gray-700 dark:hover:text-white flex items-center gap-2"
            >
              🎨 {showStylePanel ? 'Hide' : 'Show'} Form Style Settings
            </button>
            {showStylePanel && (
              <div className="mt-4 grid grid-cols-3 gap-4">
                <div>
                  <label className="text-xs font-medium text-gray-500 dark:text-slate-400 uppercase tracking-wide block mb-2">Primary Color</label>
                  <div className="flex items-center gap-2">
                    <input
                      type="color"
                      value={primaryColor}
                      onChange={e => setPrimaryColor(e.target.value)}
                      className="w-10 h-10 rounded cursor-pointer border border-gray-300 dark:border-slate-600"
                    />
                    <span className="text-sm text-gray-600 dark:text-slate-300">{primaryColor}</span>
                  </div>
                </div>
                <div>
                  <label className="text-xs font-medium text-gray-500 dark:text-slate-400 uppercase tracking-wide block mb-2">Background Color</label>
                  <div className="flex items-center gap-2">
                    <input
                      type="color"
                      value={backgroundColor}
                      onChange={e => setBackgroundColor(e.target.value)}
                      className="w-10 h-10 rounded cursor-pointer border border-gray-300 dark:border-slate-600"
                    />
                    <span className="text-sm text-gray-600 dark:text-slate-300">{backgroundColor}</span>
                  </div>
                </div>
                <div>
                  <label className="text-xs font-medium text-gray-500 dark:text-slate-400 uppercase tracking-wide block mb-2">Font Family</label>
                  <select
                    value={fontFamily}
                    onChange={e => setFontFamily(e.target.value)}
                    className="w-full px-3 py-2 border border-gray-300 dark:border-slate-600 rounded-lg text-sm bg-white dark:bg-slate-700 text-gray-900 dark:text-white outline-none"
                    style={{ fontFamily }}
                  >
                    {FONT_FAMILIES_GROUPED.map(group => (
                      <optgroup key={group.group} label={group.group}>
                        {group.fonts.map(f => (
                          <option key={f} value={f} style={{ fontFamily: f }}>{f}</option>
                        ))}
                      </optgroup>
                    ))}
                  </select>
                </div>
              </div>
            )}
          </div>
        </div>

        {/* Questions */}
        <DndContext sensors={sensors} collisionDetection={closestCenter} onDragEnd={handleDragEnd}>
          <SortableContext items={questions.map(q => q.id)} strategy={verticalListSortingStrategy}>
            <div className="flex flex-wrap gap-4">
              {questions.map((question, idx) => (
                <SortableQuestion
                  key={question.id}
                  question={question}
                  isQuiz={formType === 'Quiz'}
                  onChange={updated => updateQuestion(idx, updated)}
                  onDelete={() => deleteQuestion(idx)}
                />
              ))}
            </div>
          </SortableContext>
        </DndContext>

        {/* Add question */}
        <div className="flex flex-wrap gap-2 justify-center py-4">
          {(['ShortText', 'LongText', 'MultipleChoice', 'Checkbox', 'Dropdown'] as QuestionType[]).map(type => (
            <button
              key={type}
              onClick={() => addQuestion(type)}
              className="px-4 py-2 text-sm font-medium text-blue-600 dark:text-blue-400 border border-blue-200 dark:border-blue-700 bg-white dark:bg-slate-800 rounded-xl hover:bg-blue-50 dark:hover:bg-blue-900/20 transition-colors shadow-sm"
            >
              + {type === 'ShortText' ? 'Short Answer' : type === 'LongText' ? 'Long Answer' : type === 'MultipleChoice' ? 'Multiple Choice' : type}
            </button>
          ))}
        </div>
      </div>
    </div>
  );
}
