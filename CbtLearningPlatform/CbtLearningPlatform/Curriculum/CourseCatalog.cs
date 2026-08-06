namespace CbtLearningPlatform.Curriculum;

/// <summary>One of the 15 weeks in the academic curriculum reference (kpt_syllabus.pdf). Metadata only —
/// actual lesson content stays in Razor pages per the KEEP RAZOR FOR MVP decision (ADR, 03_DECISION_LOG.md).
/// Never store full lesson text, personal data, clinical scoring, or user progress here.</summary>
public sealed record CourseWeekDefinition(
    int Number,
    string ModuleLabel,
    string Title,
    string ShortSummary,
    CourseWeekStatus Status,
    CurriculumSafetyLevel SafetyLevel,
    string? Route,
    IReadOnlyList<string> LearningObjectives,
    IReadOnlyList<InteractiveFormat> InteractiveFormats);

/// <summary>One of the 4 curriculum blocks shown on the Weekly Course Hub — mirrors the syllabus PDF's own
/// module grouping (Модул I–IV), not an invented structure.</summary>
public sealed record CourseModule(int Number, string Title, string WeekRangeLabel, string Description);

/// <summary>Single source of truth for the 15-week curriculum reference. Only Weeks 1 and 8 have a real
/// Route so far — every other week is intentionally InPreparation/AcademicOverview/ProfessionalReviewRequired
/// with Route=null, so the hub never links to a page that doesn't exist.</summary>
public static class CourseCatalog
{
    public static IReadOnlyList<CourseModule> Modules { get; } =
    [
        new(1, "Теоретични и исторически основи", "Седмици 1–3",
            "Как възниква когнитивният модел на Бек и как е устроен."),
        new(2, "Как работи КПТ и терапевтичният процес", "Седмици 4–7",
            "Клинична оценка, терапевтичен съюз и структурата на сесията — представени академично, не като инструкция за самотерапия."),
        new(3, "Когнитивни инструменти и преструктуриране", "Седмици 8–12",
            "Автоматични мисли, когнитивни изкривявания, Сократически въпроси и работа с вярвания."),
        new(4, "Разширени техники и академичен контекст", "Седмици 13–15",
            "Специфични техники, поддържане на напредъка и по-нови направления в КПТ традицията.")
    ];

    public static IReadOnlyList<CourseWeekDefinition> Weeks { get; } =
    [
        Week(1, "Теоретични и исторически основи",
            "Как се ражда когнитивната терапия",
            "Как когнитивната терапия на Аарон Бек възниква като научен отговор на психоанализата.",
            CurriculumSafetyLevel.PublicCore, route: "/kurs/sedmica-1",
            objectives:
            [
                "Разбирате защо и как възниква когнитивната терапия.",
                "Различавате когнитивния модел от по-ранни психоаналитични обяснения."
            ],
            formats: [InteractiveFormat.InteractiveModel]),

        Week(2, "Теоретични и исторически основи",
            "Когнитивна терапия на Бек и REBT на Елис",
            "Сравнение между двете основополагащи когнитивни школи като академичен паралел, не като спор кой е прав.",
            CurriculumSafetyLevel.PublicCore, route: null,
            objectives:
            [
                "Сравнявате основните допускания на Бек и Елис.",
                "Разбирате, че различните школи подчертават различни аспекти, не че една е \"вярна\"."
            ],
            formats: [InteractiveFormat.Comparison]),

        Week(3, "Теоретични и исторически основи",
            "Архитектура на когнитивния модел",
            "Йерархията автоматични мисли → междинни вярвания → основни вярвания и моделът за бърза и рефлексивна обработка на информация.",
            CurriculumSafetyLevel.PublicCore, route: null,
            objectives:
            [
                "Разпознавате трите нива на когниция: автоматични мисли, междинни вярвания, основни вярвания.",
                "Разбирате идеята за схема като филтър на информация."
            ],
            formats: [InteractiveFormat.InteractiveModel]),

        Week(4, "Как работи КПТ и терапевтичният процес",
            "Клинична оценка и когнитивна концептуализация",
            "Общ преглед как професионалист организира информация за случай — не диагностичен инструмент за самооценка.",
            CurriculumSafetyLevel.AcademicContextOnly, route: null,
            objectives:
            [
                "Разбирате в общи линии как професионалист изгражда разбиране за случай.",
                "Осъзнавате границата между образователен преглед и реална клинична оценка."
            ],
            formats: [InteractiveFormat.AcademicOnly]),

        Week(5, "Как работи КПТ и терапевтичният процес",
            "Принципи на КПТ и терапевтичен съюз",
            "10-те основни принципа на КПТ и ролята на сътрудничеството между терапевт и клиент.",
            CurriculumSafetyLevel.PublicWithAdaptation, route: null,
            objectives:
            [
                "Запознавате се с основните принципи на КПТ.",
                "Разбирате ролята на сътрудничеството в терапевтичния процес."
            ],
            formats: [InteractiveFormat.GuidedDemonstration]),

        Week(6, "Как работи КПТ и терапевтичният процес",
            "Структура на терапевтичната сесия",
            "Информационен преглед как изглежда типична КПТ сесия — не инструкция за самотерапия.",
            CurriculumSafetyLevel.PublicWithAdaptation, route: null,
            objectives:
            [
                "Разбирате общата структура на типична КПТ сесия.",
                "Различавате информационен преглед от инструкция за самотерапия."
            ],
            formats: [InteractiveFormat.GuidedDemonstration]),

        Week(7, "Как работи КПТ и терапевтичният процес",
            "Поведенческа активация",
            "Връзката между действие и настроение и как малки стъпки могат да противодействат на бездействието.",
            CurriculumSafetyLevel.PublicWithAdaptation, route: null,
            objectives:
            [
                "Разбирате връзката между поведение и настроение.",
                "Запознавате се с идеята за малки, постепенни стъпки на действие."
            ],
            formats: [InteractiveFormat.Simulator]),

        Week(8, "Когнитивни инструменти и преструктуриране",
            "Автоматични мисли и емоции",
            "Прилагане на модела Ситуация → Мисъл → Емоция → Поведение върху конкретни примери.",
            CurriculumSafetyLevel.PublicCore, route: "/kurs/sedmica-8",
            objectives:
            [
                "Прилагате модела Ситуация → Мисъл → Емоция → Поведение върху примери.",
                "Разграничавате автоматична мисъл от чувството, което я следва."
            ],
            formats: [InteractiveFormat.InteractiveModel, InteractiveFormat.Simulator, InteractiveFormat.KnowledgeCheck]),

        Week(9, "Когнитивни инструменти и преструктуриране",
            "Когнитивни изкривявания и дневник на мислите",
            "Типични \"грешки в мисленето\" и структуриран начин за записване на мисли.",
            CurriculumSafetyLevel.PublicWithAdaptation, route: null,
            objectives:
            [
                "Разпознавате няколко типични когнитивни изкривявания.",
                "Запознавате се с идеята за структурирано записване на мисли."
            ],
            formats: [InteractiveFormat.Simulator]),

        Week(10, "Когнитивни инструменти и преструктуриране",
            "Сократически въпроси",
            "Метод за изследване на мисълта чрез внимателни, отворени въпроси, а не чрез спор.",
            CurriculumSafetyLevel.PublicCore, route: null,
            objectives:
            [
                "Разбирате Сократическия въпрос като метод за изследване, не за убеждаване.",
                "Практикувате разглеждане на алтернативни гледни точки върху неутрален пример."
            ],
            formats: [InteractiveFormat.GuidedDemonstration]),

        Week(11, "Когнитивни инструменти и преструктуриране",
            "Междинни вярвания",
            "Нагласи и правила, които стоят зад повтарящи се автоматични мисли — представени образователно.",
            CurriculumSafetyLevel.ProfessionalReviewRequired, route: null,
            objectives:
            [
                "Разбирате какво е междинно вярване (нагласа/правило).",
                "Виждате връзката между междинни вярвания и повтарящи се автоматични мисли."
            ],
            formats: [InteractiveFormat.InteractiveModel]),

        Week(12, "Когнитивни инструменти и преструктуриране",
            "Основни вярвания и схеми",
            "Най-дълбокото ниво на когнитивния модел — обзорно, академично представяне.",
            CurriculumSafetyLevel.AcademicContextOnly, route: null,
            objectives:
            [
                "Запознавате се с понятието основно вярване/схема.",
                "Разбирате трите широки категории — безпомощност, необичаемост, безполезност."
            ],
            formats: [InteractiveFormat.AcademicOnly]),

        Week(13, "Разширени техники и академичен контекст",
            "Вземане на решения и поведенчески техники",
            "Инструменти за преценка на решения (decision balance, отговорност) и поетапно, безопасно изправяне пред трудни ситуации. Смесен обхват — вижте simulator opportunity matrix в 24_IMPLEMENTATION_ROADMAP.md за разбивка по под-тема; поведенческото излагане (exposure) остава най-строго ограничено, докато не мине отделен професионален преглед.",
            CurriculumSafetyLevel.NotEligibleForSelfGuidedSimulator, route: null,
            objectives:
            [
                "Запознавате се с инструменти за преценка на решения.",
                "Разбирате принципа на поетапност при изправяне пред трудни ситуации."
            ],
            formats: [InteractiveFormat.Simulator]),

        Week(14, "Разширени техники и академичен контекст",
            "Домашна работа, приключване и превенция на рецидив",
            "Как наученото се поддържа след края на структурирана работа.",
            CurriculumSafetyLevel.ProfessionalReviewRequired, route: null,
            objectives:
            [
                "Разбирате защо поддържането на наученото е отделна стъпка.",
                "Запознавате се с идеята за план за трудни периоди."
            ],
            formats: [InteractiveFormat.StaticVisualization]),

        Week(15, "Разширени техники и академичен контекст",
            "Трета вълна и възстановително-ориентирана терапия",
            "Кратък обзор на по-новите направления в КПТ традицията.",
            CurriculumSafetyLevel.AcademicContextOnly, route: null,
            objectives:
            [
                "Получавате кратък обзор на по-новите направления в КПТ.",
                "Разбирате, че полето продължава да се развива."
            ],
            formats: [InteractiveFormat.AcademicOnly])
    ];

    private static CourseWeekDefinition Week(
        int number, string moduleLabel, string title, string shortSummary,
        CurriculumSafetyLevel safetyLevel, string? route,
        IReadOnlyList<string> objectives, IReadOnlyList<InteractiveFormat> formats) =>
        new(number, moduleLabel, title, shortSummary,
            CurriculumLabels.DeriveStatus(safetyLevel, route), safetyLevel, route, objectives, formats);
}
