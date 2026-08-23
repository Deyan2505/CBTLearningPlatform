# 24 — Implementation Roadmap

**Собственическо одобрение:** получено на 2026-07-30 за всичките 5 предпоставки (MVP, стек — с .NET 10 LTS корекция, IA, data model, prototype решение) — виж ADR-007/ADR-008 в `03_DECISION_LOG.md`.

**STEP-1.1 статус: `COMPLETE`** (2026-07-30, Сесия 6). .NET 10 SDK инсталиран автономно; Blazor Web App solution `CbtLearningPlatform` създаден, build-проверен (0 предупреждения/грешки), `dotnet run` реално проверен (HTTP 200). Пълни доказателства в `02_CURRENT_STATUS.md`, `10_SESSION_LOG.md`, `26_SKILL_USAGE_LOG.md`.

**STEP-1.2 статус: `COMPLETE`** (2026-07-30, Сесия 7). Git repository инициализиран в правилния project root, официален `.NET .gitignore` темплейт създаден и валидиран, branch `main`, Git identity зададена локално (собственикът потвърди email), baseline commit създаден с цялата одобрена baseline основа.

**STEP-1.3 статус: `CI CONFIGURATION COMPLETE — REMOTE RUN PENDING`** (2026-07-30, Сесия 8). `.github/workflows/ci.yml` създаден — restore + Release build, `permissions: contents: read`, SDK от `global.json`, без caching. Security review и структурен YAML преглед чисти; локален Release build 0/0. Реален GitHub Actions run **не е възможен** — repository няма remote (извън обхвата на тази стъпка).

**STEP-1.4 статус: `COMPLETE — REMOTE CI RUN PENDING`** (2026-07-30, Сесия 9). xUnit test project `CbtLearningPlatform.Tests` създаден (официален template, `net10.0`), добавен към solution, project reference към host; 2 реални инфраструктурни теста, и двата passing. CI workflow разширен с `Test` стъпка. Локален build+test: 0/0, 2/2 passed.

**STEP-1.5 статус: `COMPLETE — REMOTE CI RUN PENDING`** (2026-07-30, Сесия 10). `ErrorBoundary` + българска `Error.razor` + преведен `#blazor-error-ui` банер. Server-side exception handling вече беше в темплейта (`UseExceptionHandler`) — не дублиран. Реално симулирано изключение в истинска Production среда потвърди безопасно поведение (HTTP 500, без изтекли детайли). 2 нови теста, общо 4/4 passing. Вижте Phase 1 Remaining Steps Review (`10_SESSION_LOG.md`, Сесия 10) за препоръка относно STEP-1.6 vs. директен преход към Фаза 2.

Детайлните стъпки по-долу покриват Фаза 1 (техническа основа) — непосредствено следващата фаза. Фази 2–9 остават на нивото на детайл в `01_MASTER_PLAN.md`, до момента на реалното им започване (тогава ще получат същото ниво на детайл).

## Фаза 1 — Техническа основа: стъпки

### STEP-1.1 — Инициализация на Blazor Web App solution
- **Цел:** работещ, празен, стартиращ проект.
- **Обхват:** `dotnet new blazor` solution (target framework **`net10.0`**, изрично проверено, не по подразбиране на инсталирания SDK) с основна папкова структура (Components/Pages, Components/Layout, Content, wwwroot).
- **Зависимости:** одобрен технологичен стек (`20_TECHNOLOGY_DECISION.md`, .NET 10 LTS) — **и наличен .NET 10 SDK + Visual Studio на машината, разработваща проекта.**
- **Засегнати файлове:** нов solution (~10-15 нови файла от шаблона).
- **Acceptance criteria:** `dotnet run` показва началната страница локално; `.csproj` сочи `<TargetFramework>net10.0</TargetFramework>`.
- **Тестове:** N/A (скеле).
- **Рискове:** грешен .NET SDK версия инсталирана локално — проверка преди старт (виж `02_CURRENT_STATUS.md` Environment Check — **на 2026-07-30 SDK липсва напълно, стъпката е `BLOCKED`**).
- **Документация:** `10_SESSION_LOG.md` запис.
- **Definition of Done:** проектът се компилира и стартира с една команда от чист clone.

### STEP-1.2 — Git repository и .gitignore
- **Цел:** версионен контрол от самото начало.
- **Обхват:** `git init`, `.gitignore` (стандартен .NET темплейт + IDE файлове), първи commit.
- **Зависимости:** STEP-1.1.
- **Acceptance criteria:** `git status` чист след build; няма `bin/`/`obj/` в проследяването.
- **Definition of Done:** repo съществува, история започва чисто.
- **Статус (2026-07-30, Сесия 7): `COMPLETE`.** `git init`/`.gitignore` изпълнени и валидирани; acceptance criteria за проследяването изпълнено (`bin/`/`obj/` игнорирани, потвърдено с `git check-ignore`). Git identity зададена локално (собственикът потвърди email при изрична заявка); baseline commit създаден. Definition of Done постигнато — repo съществува, история започва чисто.

### STEP-1.3 — Основен CI build
- **Цел:** автоматична проверка при всеки push.
- **Обхват:** GitHub Actions (или сравним) workflow — restore + build + test.
- **Зависимости:** STEP-1.1, STEP-1.2.
- **Acceptance criteria:** push към main тригерва успешен build.
- **Definition of Done:** CI зелен badge, реално тестван с push.
- **Статус (2026-07-30, Сесия 8): `CI CONFIGURATION COMPLETE — REMOTE RUN PENDING`.** `test` частта на обхвата съзнателно отложена — няма test project (той е предмет на STEP-1.4); workflow-ът ще бъде разширен с `dotnet test`, когато проектът реално съществува, вместо фиктивна команда сега. Acceptance criteria/Definition of Done, изискващи push и зелен badge, остават `PENDING` — repository няма GitHub remote (създаването му е извън обхвата на тази стъпка).

### STEP-1.4 — Тестов проект (xUnit)
- **Цел:** основа за automated tests.
- **Обхват:** нов xUnit проект, референция към основния; един тривиален тест за проверка на настройката.
- **Зависимости:** STEP-1.1.
- **Acceptance criteria:** `dotnet test` минава.
- **Definition of Done:** CI изпълнява тестовия проект (интегрира се в STEP-1.3).
- **Статус (2026-07-30, Сесия 9): `COMPLETE — REMOTE CI RUN PENDING`.** Вместо един тривиален тест — 2 честни инфраструктурни теста (target framework verification чрез `TargetFrameworkAttribute` на компилирания test assembly — не `AppContext.TargetFrameworkName`, който отразява entry assembly на VSTest testhost, не самия test проект; host assembly loadability). CI интеграцията изпълнена (`.github/workflows/ci.yml` разширен с `Test` стъпка). Реален GitHub run остава `PENDING` — няма remote.

### STEP-1.5 — Централизирана обработка на грешки
- **Цел:** потребителят никога не вижда суров stack trace.
- **Обхват:** глобален error boundary (Blazor) + custom error страница.
- **Зависимости:** STEP-1.1.
- **Acceptance criteria:** симулирано изключение показва приятелска страница, не suров trace.
- **Тестове:** unit тест за error boundary поведение.
- **Definition of Done:** проверено ръчно + автоматизиран тест.
- **Статус (2026-07-30, Сесия 10): `COMPLETE — REMOTE CI RUN PENDING`.** Acceptance criteria проверено буквално ръчно: временен diagnostic endpoint (премахнат преди commit) хвърлен в истинска Production среда (не Development — `launchSettings.json` override избегнат чрез директно стартиране на compiled DLL) → HTTP 500, приятелско българско съобщение, 0 изтекли технически детайли. Автоматизиран тест: 2 reflection-базирани теста (съществуване на Error компонента + архитектурна гаранция, че той няма property от тип `Exception`).

### STEP-1.6 — Linting/formatting конфигурация
- **Цел:** консистентен код стил.
- **Обхват:** `.editorconfig`, базови Roslyn analyzer правила.
- **Зависимости:** STEP-1.1.
- **Acceptance criteria:** `dotnet format --verify-no-changes` минава на чист проект.
- **Definition of Done:** интегрирано в CI (STEP-1.3) като проверка.
- **Статус (2026-08-01, Сесия 11): `DEFERRED — REQUIRED BEFORE FIRST MAJOR FEATURE MERGE OR REMOTE PR`.** Собственическо решение да се премине директно към Фаза 2. Не блокира UI работа; задължителна преди първи голям feature merge, remote pull request или deployment readiness review.

## Преход към Фаза 2 (дизайн система) — предварителни условия

Фаза 2 не започва, докато Фаза 1 STEP-1.1…1.6 не са `Done` и потвърдени в `02_CURRENT_STATUS.md`. **Изключение (2026-08-01, Сесия 11):** собственикът изрично реши STEP-1.6 да остане `DEFERRED` (не блокира UI работа) и Фаза 2 да започне без нея — виж статус на STEP-1.6 по-горе.

## Фаза 2 — Дизайн система и UX основа: стъпки

### STEP-2.1 — Design system foundation (tokens + базови компоненти)
- **Цел:** последователна, достъпна визуална основа преди писане на съдържание.
- **Обхват:** CSS custom properties (color roles, typography, spacing, shape, layout); базови компоненти — бутони, връзки, карти, callout/disclaimer banner, navigation container (header + footer + skip link), form field foundation; `MainLayout.razor` пренаписан с реална навигация по одобрената IA; `Home.razor` пренаписана с честно, минимално съдържание (не темплейтен placeholder).
- **Зависимости:** Фаза 1 (STEP-1.1–1.5; STEP-1.6 съзнателно отложена).
- **Skill:** `ui-ux-pro-max` (SKILL-046) — `--design-system` + `--domain color/typography/ux` заявки за starting point, адаптирани ръчно (виж `26_SKILL_USAGE_LOG.md` за точните приложени/отхвърлени препоръки).
- **Acceptance criteria:** споделените компоненти реално се използват на `Home.razor` и `NotFound.razor`; build 0/0; тестове зелени; HTTP smoke test потвърждава структурата (nav/header/footer/disclaimer/skip-link/lang="bg") в реално сервирания HTML.
- **Definition of Done (тази стъпка):** design tokens + основни компоненти съществуват и се използват на поне 1 реална страница (`Home.razor`); пълният Phase 2 критерий ("поне 2 реални страници") се постига кумулативно със следващи стъпки/Фаза 3 съдържателни страници.
- **Статус (2026-08-01, Сесия 11): `COMPLETE`.** Виж `10_SESSION_LOG.md` за пълни детайли.

### STEP-2.2 — Real page wireframes and navigation
- **Цел:** поне 2 реални страници, използващи споделената компонентна библиотека (изпълнява Phase 2 критерия за завършване).
- **Обхват:** `Home.razor` пренаписана (hero/процес/модули preview/образователна граница); нова `/programa` страница (модулен каталог); нов `ModuleCard.razor` shared component; nav active state (`NavLink`).
- **Зависимости:** STEP-2.1.
- **Skill:** `ui-ux-pro-max` — landing/UX справки, адаптирани ръчно (отхвърлени testimonials/social-proof/sticky-nav/video-hero модели — виж `26_SKILL_USAGE_LOG.md`).
- **Решение за модулните карти:** Модул 1 и Модул 2 показани с честен статус "в процес на подготовка", **без** линкове към несъществуващи lesson страници — за разлика от header/footer nav (приета практика от STEP-2.1 да сочи към бъдещи routes), модулна карта директно на страница представлява по-силно обещание за клик "сега", затова не се линква преждевременно.
- **Acceptance criteria:** build 0/0; 9/9 теста; `/` и `/programa` връщат HTTP 200; nav active state коректен (`aria-current="page"`); няма dead links; disclaimer видим на двете страници.
- **Статус (2026-08-01, Сесия 12): `COMPLETE`.** Phase 2 критерий за завършване ("споделена библиотека, използвана на поне 2 реални страници") изпълнен. Visual QA: `PROVISIONAL — SCREENSHOT REVIEW PENDING` (HTTP/структурна проверка извършена; няма достъпен browser/screenshot инструмент в тази сесия).
- **Следваща стъпка в Фаза 2/3 (не изпълнена автоматично):** реално съдържание за `/kpt` и модулните lesson страници — изисква `07_CONTENT_GOVERNANCE.md` цикъл, ново извикване.

## Фаза 3 — Система за обучително съдържание: стъпки

### STEP-3.1 — Първи реален обучителен content slice
- **Цел:** малък, напълно използваем обучителен поток: Начало → Програма → Какво е КПТ → Модул → урок, който установява стандарта за всички следващи уроци.
- **Обхват:** `/kpt` (публична страница); `/programa/modul-2` (module overview); `/programa/modul-2/situacia-misal-emocia-povedenie` (първи реален урок); `LearningObjectives.razor` + `SourceReferences.razor` (reusable, реално използвани 2×).
- **Зависимости:** Фаза 2 (STEP-2.1, STEP-2.2).
- **Content governance:** REQ-CLIN-003 (SRC-041 Гл. 3, "Бек 1964; Елис 1962" — не изисква source review, добре установен модел) + REQ-CONT-002/005 (урочен шаблон, флагмански модул като първи урок). REQ-CLIN-008 (10 принципа) и REQ-CLIN-009 (evidence-base статистика от 2006) — **съзнателно изключени** от тази стъпка (Deferred/нужда от актуализирана проверка). Пример в урока — оригинален, неутрален, не адаптиран от `code_artifact.html`.
- **Архитектурна забележка (отворен въпрос, не решен в тази стъпка):** съдържанието е директно в Razor markup, не през Markdown/JSON parsing pipeline, буквално предвиден в Master Plan Фаза 3 ("зареждане и парсване на съдържание от файлове"). ADR-003 (без база данни) не е нарушено, но файлов parsing pipeline остава отворено решение — препоръка: реши се след 2–4 реални урока, когато шаблонът е доказан.
- **Решение за именуване:** инструкцията за тази стъпка илюстративно посочи route `/programa/modul-1` за примерния урок, но описаното съдържание (S→M→E→P модел) е точно одобреният Модул 2 (флагмански) по `18_INFORMATION_ARCHITECTURE.md`/REQ-CONT-005 — реализирано с коректната одобрена номерация вместо примерния route.
- **Реален бъг, открит и коригиран по време на изпълнение:** `/kpt` и урок страницата първоначално нямаха `DisclaimerCallout` (само списък "какво не е"), въпреки изричното правило в `23_CLINICAL_SAFETY_BOUNDARIES.md` за видим disclaimer на **всяка** страница с психологическо съдържание, не само началната. Открито чрез реален HTTP smoke test, коригирано, добавен регресионен тест (`PsychologicalContentPage_IncludesDisclaimerCallout`).
- **Acceptance criteria:** build 0/0; 21/21 теста; всички нови routes връщат HTTP 200; несъществуващ lesson route → friendly NotFound; module overview сочи към реалния първи урок (без dead link); неподкрепената категоризация отсъства; disclaimer видим на всички съдържателни страници.
- **Статус (2026-08-01, Сесия 13): `COMPLETE`.** `REQUIRES PROFESSIONAL REVIEW` маркирано (Razor коментари + `13_REQUIREMENTS_TRACEABILITY.md`) — не публикувано за реални потребители без клиничен рецензент (RISK-010). Visual QA: `PROVISIONAL — SCREENSHOT REVIEW PENDING`.
- **Следваща стъпка (не изпълнена автоматично):** Google Fonts CDN решение (deferred), STEP-1.6 (deferred), или следващ урок/модул — собственическо решение, изисква ново извикване.

### STEP-3.2 — Втори реален урок от Модул 2
- **Цел:** втори реален урок, надграждащ модела от Урок 1, потвърждаващ повторяемостта на урочния шаблон.
- **Обхват:** `/programa/modul-2/avtomatichni-misli` ("Автоматични мисли"); актуализиран `Modul2.razor` overview (втора `ModuleCard`); актуализиран Урок 1 (реален линк към Урок 2 вместо "предстои").
- **Зависимости:** STEP-3.1.
- **Content governance:** тема — SRC-041 Гл. 9 ("Идентифициране на автоматични мисли"), REQ-CONT-001/002. Изтеглена от одобрения content map запис "Модул 3 — Автоматични мисли" (`18_INFORMATION_ARCHITECTURE.md`), реализирана като Урок 2 на флагманския Модул 2 по REQ-CONT-002 ("2–4 lessons in the flagship module"), не като отделна Модул 3 страница — консистентно с решението от STEP-3.1.
- **Нов пример:** различна тема от Урок 1 (готвене по нова рецепта, вместо съобщение без отговор) — демонстрира разграничение мисъл/чувство.
- **Acceptance criteria:** build 0/0; 25/25 теста; всички routes 200; Урок 1 вече линква реално към Урок 2 (без dead "предстои"); Урок 2 честно маркира Урок 3 като "предстои"; disclaimer на всички съдържателни страници; неподкрепената категоризация отсъства.
- **Статус (2026-08-01, Сесия 14): `COMPLETE`.** `REQUIRES PROFESSIONAL REVIEW` маркирано. Visual QA: `PROVISIONAL — SCREENSHOT REVIEW PENDING`.
- **Markdown/JSON pipeline препоръка:** с 2 реални урока структурното повторение личи ясно, но първоначалната препоръка (3–4 урока преди решение) все още не е достигната — препоръка: 1 още урок преди окончателно решение.
- **Следваща стъпка (не изпълнена автоматично):** трети урок, Google Fonts CDN решение, Markdown/JSON pipeline решение, или STEP-1.6 — собственическо решение, изисква ново извикване.

### STEP-3.3 — Трети урок и Content Pipeline Decision Gate
- **Цел:** трети реален урок (горна практическа граница на флагманския модул за MVP) + окончателен, недвусмислен анализ дали е нужен Markdown/JSON content pipeline.
- **Обхват:** `/programa/modul-2/emocii-i-telesni-reaktsii` ("Емоции и телесни реакции"); актуализиран `Modul2.razor` (трета `ModuleCard`); актуализиран Урок 2 (реален линк към Урок 3).
- **Зависимости:** STEP-3.2.
- **Архитектурна реконсилиация (ADR-009, `03_DECISION_LOG.md`):** формализирано решение Вариант C — Модул 2 официално поема базовото ниво на темите от бъдещите Модул 3 ("Автоматични мисли") и Модул 5 ("Емоции и телесни реакции"); тези бъдещи модули се стесняват (Модул 3 → задълбочено записване на мисли/мост към thought record; Модул 5 → задълбочена емоционална регулация), за да не дублират вече наученото. `18_INFORMATION_ARCHITECTURE.md` актуализиран с анотации на засегнатите модули.
- **Content governance:** SRC-041 Гл. 10 ("Идентифициране на емоции"), REQ-CONT-001/002. Нов пример (закъснение в трафик), различен от Урок 1 (съобщение) и Урок 2 (готвене).
- **Acceptance criteria:** build 0/0; 30/30 теста; всички routes 200; Урок 2 вече линква реално към Урок 3; Урок 3 честно маркира липсващ Урок 4 като "предстои"; disclaimer навсякъде; неподкрепената категоризация отсъства.
- **Статус (2026-08-01, Сесия 15): `COMPLETE`.**
- **Content Pipeline Decision Analysis (сравнение на 3 реални урока, без имплементация):**
  - **Повтарящи се елементи:** route metadata, `REQUIRES PROFESSIONAL REVIEW` коментар, `PageTitle`/`h1`, `LearningObjectives` (3 цели всеки), връзка с предходния урок, "Основно обяснение" (2 абзаца), "Пример от ежедневието" (структурирана `.card`/`.stack` markup), опционална сравнителна секция, `callout--info` "Забележете", "Проверка на разбирането" (3–4 въпроса), "Обобщение" (4–5 извода), `DisclaimerCallout`, навигация, `SourceReferences`.
  - **Реални разлики:** Урок 1 има 2 паралелни примера (`.stack` с 2 `.card`), Уроци 2–3 имат по 1; изричен "Сравнение" heading само в Урок 2; брой въпроси/изводи варира леко (3–4/4–5).
  - **Оценка по критерии:** Сложност (Razor: ниска, вече доказана; Markdown: средна — изисква custom block синтаксис за `.card`/`.callout`/`.stack`, за да не се загуби design system интеграцията). Тестируемост (Razor: 30/30 работещи source-базирани теста без триене). Авторска редакция (полза от Markdown е теоретична — единственият автор в момента е през Claude Code сесии, не отделен non-technical редактор). Source traceability/clinical review workflow: идентични и при двата подхода. Performance: Razor е предварително компилиран, без runtime parsing. Migration cost: нисък сега (3 урока), но нараства с всеки следващ. Риск от overengineering: **висок** точно сега — REQ-CONT-002 таванира флагманския модул на 2–4 урока и той вече е при горната граница; pipeline за максимум 1 бъдещ урок в тази конкретна граница не носи пропорционална стойност.
  - **Решение:** `KEEP RAZOR FOR MVP`. Тясната връзка на съдържанието с споделените компоненти (`LearningObjectives`/`SourceReferences`/`DisclaimerCallout`/`ModuleCard`) и вградената structural markup (карти/callouts) прави Markdown миграцията в момента нетна загуба на простота без реална полза — 3 урока е малък обем, а REQ-CONT-002 ограничава по-нататъшния растеж точно на този модул. Решението подлежи на преразглеждане, когато стартира съдържание за Модул 1 или друг бъдещ модул извън капацитета на флагманския Модул 2.
- **Следваща стъпка (не изпълнена автоматично):** Google Fonts CDN решение, Модул 1 съдържание, или STEP-1.6 — собственическо решение, изисква ново извикване.

### STEP-3.4 — Модул 1 overview и първи реален урок
- **Цел:** реализация на Модул 1 ("Какво представлява КПТ") — уводния модул преди флагманския Модул 2.
- **Обхват:** `/programa/modul-1` (overview); `/programa/modul-1/kakvo-e-kpt` (единствения одобрен урок — IA описва Модул 1 като едно цяло, не многоурочна структура); актуализирани `Programa.razor`/`Home.razor` (реален линк вместо disabled state).
- **Зависимости:** STEP-2.2 (`Programa.razor` съществува), независим от STEP-3.1–3.3 (различен модул).
- **Архитектурна проверка:** без конфликт между IA и Requirements Traceability — REQ-CLIN-009 (evidence base) позволява общо, добре подкрепено твърдение, докато специфичната 2006 статистика изрично се нуждае от актуализирана проверка (същото ограничение, приложено вече в STEP-3.1 за `/kpt`). Модул 1 остава честно едноурочен — не е измислена нова подструктура.
- **Content governance:** SRC-041 Гл. 1, REQ-CONT-001, REQ-CLIN-009 (само общата claim). Нов пример (приятел споделя тревога), различен по тема от S-T-E-B примерите в Модул 2 — илюстрира границата образование/терапия, не самия модел (Модул 1 не преподава модела, това е Модул 2).
- **Acceptance criteria:** build 0/0; 38/38 теста; всички routes 200 (вкл. Модул 1 overview + урок); `Programa.razor`/`Home.razor` вече без disabled Модул 1 карта; Урок 1 линква реално напред към Модул 2 (Модул 1 няма урок 2); disclaimer навсякъде; неподкрепената категоризация отсъства.
- **Статус (2026-08-01, Сесия 16): `COMPLETE`.** `REQUIRES PROFESSIONAL REVIEW` маркирано. Visual QA: `PROVISIONAL — SCREENSHOT REVIEW PENDING`. Content Pipeline решението `KEEP RAZOR FOR MVP` — непроменено, потвърдено отново приложимо (Модул 1 съдържанието се вписва в същия established Razor+components модел без триене).
- **Следваща стъпка (не изпълнена автоматично):** Google Fonts CDN решение, STEP-1.6, или бъдещо съдържание извън Модул 1/2 (напр. Модул 14, речник, ЧЗВ) — собственическо решение, изисква ново извикване.

## Visual Direction Correction Checkpoint — dark-first UI + интерактивни учебни визуализации

- **Цел:** собственикът направи първи реален визуален преглед след STEP-3.4 и отхвърли текущата визуална посока (STEP-2.1/2.2 палитра/компоненти) като не-финална — твърде статична, недостатъчна визуална йерархия, недостатъчна интерактивност. Тази checkpoint стъпка коригира посоката, без да променя клинично съдържание или да добавя нови модули/уроци.
- **Обхват:** dark-first, multi-level, не-чисто-черен design system (запазва light темата през session-only toggle, без cookies/localStorage/server storage); reusable интерактивна CBT model диаграма (Interactive WebAssembly, на `/kpt` + Модул 2 Урок 1); малък интерактивен interpretation пример (без scoring/storage) на `/kpt`; learning-path визуализация, заменяща плоския `ModuleCard` списък на Home/Programa; целенасочени page-level refinements (Kpt, Modul2Lesson1, Home, Programa).
- **Зависимости:** Фаза 2 (STEP-2.1/2.2), Фаза 3 (STEP-3.1–3.4) — коригира визуалния слой върху вече съществуващото реално съдържание, не добавя ново съдържание.
- **Skill:** `ui-ux-pro-max` (SKILL-046) — целенасочени dark-theme/accessibility заявки; 2 предложени "dark" стила (pure-black/neon OLED; glassmorphism/blur/glow cinema) изрично отхвърлени като несъвместими с not-pure-black/no-decorative-effects ограниченията — виж `26_SKILL_USAGE_LOG.md`.
- **Acceptance criteria:** build 0/0; 56/56 теста; реален HTTP smoke test на fresh инстанция потвърждава `data-theme="dark"` на initial HTML, всички нови компонентни маркери (theme-toggle/cbt-diagram/interpretation-example/learning-path), prerendered WASM markup, light override в CSS, WCAG AA contrast (реално изчислен, не на око), 0 disabled карти, disclaimer/forbidden-term проверки непроменени.
- **Статус (2026-08-01, Сесия 17): `TECHNICAL QA COMPLETE — OWNER VISUAL APPROVAL PENDING`.** Работата е функционално завършена и технически проверена, но **умишлено не е commit-ната** — собственическата инструкция изисква реален визуален преглед (не само HTTP 200) преди какъвто и да е commit. Собственикът трябва да прегледа `http://localhost:5055` (сървърът в момента е спрян — изисква рестарт за преглед) по чеклист (home/programa/kpt/диаграма/пример/desktop/mobile/dark/light/навигация/четивност на урок), да даде конкретна обратна връзка, след което Claude Code коригира и едва тогава прави един-единствен commit.
- **Следваща стъпка (не изпълнена автоматично):** собственически визуален преглед → конкретна обратна връзка → корекции → commit. Не се стартира thought record упражнение и не се прави commit преди тази обратна връзка.

## Weekly Course Hub + Simulator Foundation Checkpoint — dark-first redesign продължение (ADR-010)

- **Цел:** нов, дългосрочен организационен слой (`/kurs`) над съществуващото Модул 1/2 съдържание — 15-седмичен curriculum reference (`kpt_syllabus.pdf`), progressive disclosure, симулатор-богато обучение като постоянни принципи, реализирани върху вече некомитнатата Visual Direction Correction работа (Сесия 17), не вместо нея.
- **Обхват:** curriculum metadata модел (`Curriculum/CourseCatalog.cs`); curriculum safety classification за всичките 15 седмици; sidebar+top-bar application shell (структурно вдъхновен от собственически reference, без брандинг/gamification); `ProgressiveExplanation` (native `<details>`, 3 употреби); публичен `/kurs` hub (15-елементен списък, честни статус етикети, 0 dead links); представителна `/kurs/sedmica-8` (пълен 8-секционен template, композира върху трите реални Модул 2 урока); нов `CategorizationCheck` interactive knowledge-check (reveal-style, без scoring). Останалите 14 седмици и целият останал simulator каталог — `Deferred`.
- **Зависимости:** Фаза 3 (Модул 1/2 реално съдържание), Visual Direction Correction checkpoint (dark-first design system, `CbtModelDiagram`, `InterpretationExample`).
- **Skill:** `ui-ux-pro-max` — targeted заявки за weekly hub/sidebar/progressive disclosure; двата "dark" стилови резултата (OLED, Cinema Mobile glassmorphism) отхвърлени отново, идентично на Сесия 17 — виж `26_SKILL_USAGE_LOG.md`.
- **Acceptance criteria:** build 0/0; 81/81 теста; всички routes 200 (вкл. `/kurs`, `/kurs/sedmica-8`); несъществуваща седмица → 404; 0 dead links в week list; 0 ECTS/институционални claims; всички интерактивни компонентни маркери реално prerendered; disclaimer/source references никога скрити зад progressive disclosure.
- **Статус (2026-08-01, Сесия 18): `WEEKLY HUB AND SIMULATOR FOUNDATION READY — OWNER VISUAL REVIEW REQUIRED`.** Некомитнато, натрупано върху некомитнатата Сесия 17 работа. Приложението е оставено стартирано на `http://localhost:5055` за собственически преглед на двата checkpoint-а заедно.
- **Следваща стъпка (не изпълнена автоматично):** собственически визуален преглед → обратна връзка → корекции → един commit, покриващ и двата checkpoint-а. Не се строи допълнителна седмица/симулатор без ново извикване.

### Simulator Opportunity Matrix

Пълен каталог на разгледаните interactive/simulator възможности за 15-те седмици (Section 13/14 от Сесия 18 инструкцията). Категории: `SIMULATOR` / `INTERACTIVE MODEL` / `COMPARISON` / `GUIDED DEMONSTRATION` / `KNOWLEDGE CHECK` / `STATIC VISUALIZATION` / `ACADEMIC ONLY`. Реализирани в тази стъпка: **само** редовете, маркирани `IMPLEMENTED` (Седмица 8 × 3). Всички останали — `CATALOGUED, NOT BUILT`, чакат бъдещо, отделно извикване и собствен content governance цикъл преди имплементация.

| Седмица | Възможност | Тип | Safety level | Изисква проф. преглед? | Лични данни? | Изцяло локална? | MVP приоритет | Статус |
|---|---|---|---|---|---|---|---|---|
| 1 | Интерактивна времева линия (психоанализа→емпиричен модел) | INTERACTIVE MODEL | PUBLIC CORE | Не | Не | Да | P2 | CATALOGUED, NOT BUILT |
| 2 | Бек срещу Елис сравнение | COMPARISON | PUBLIC CORE | Не | Не | Да | P2 | CATALOGUED, NOT BUILT |
| 3 | Йерархия (автоматични мисли→междинни→основни) + бърза/рефлексивна обработка | INTERACTIVE MODEL | PUBLIC CORE | Не | Не | Да | P1 | CATALOGUED, NOT BUILT |
| 4 | Демонстрация как професионалист организира информация (без лична история) | ACADEMIC ONLY | ACADEMIC CONTEXT ONLY | Да (за какъвто и да е бъдещ разширен вариант) | Не (по дизайн — забранено въвеждане) | Да | P3 | CATALOGUED, NOT BUILT |
| 5 | Как изглежда сътрудничеството в КПТ | GUIDED DEMONSTRATION | PUBLIC WITH ADAPTATION | Не | Не | Да | P2 | CATALOGUED, NOT BUILT |
| 6 | Walkthrough на структура на сесия (не self-therapy) | GUIDED DEMONSTRATION | PUBLIC WITH ADAPTATION | Не | Не | Да | P2 | CATALOGUED, NOT BUILT |
| 7 | Activity–mood visualizer / basic planning demo | SIMULATOR | PUBLIC WITH ADAPTATION (проф. преглед за самия planner) | Да | Потенциално (ако въвежда лична активност) | Да | P2 | CATALOGUED, NOT BUILT |
| 8 | CBT model simulator | INTERACTIVE MODEL | PUBLIC CORE | Не | Не | Да | P0 | **IMPLEMENTED** (`CbtModelDiagram`, вече от Сесия 17) |
| 8 | Interpretation branching | SIMULATOR | PUBLIC CORE | Не | Не | Да | P0 | **IMPLEMENTED** (`InterpretationExample`, вече от Сесия 17) |
| 8 | Thought-versus-emotion sorter | KNOWLEDGE CHECK | PUBLIC CORE | Не | Не | Да | P0 | **IMPLEMENTED** (`CategorizationCheck`, ново тази стъпка) |
| 9 | Simplified thought record | SIMULATOR | PUBLIC WITH ADAPTATION | Да (отделен content/safety review) | Да (по дизайн, ако някога се въведе) | Да | P0 (бъдещо, EPIC-004) | CATALOGUED, NOT BUILT — изрично забранено за тази стъпка |
| 9 | Distortion explorer | SIMULATOR | PUBLIC WITH ADAPTATION | Да | Не | Да | P1 | CATALOGUED, NOT BUILT |
| 9 | Fact-versus-interpretation activity | SIMULATOR | PUBLIC WITH ADAPTATION | Да | Не | Да | P2 | CATALOGUED, NOT BUILT |
| 10 | Socratic question explorer / alternative-perspective demo | GUIDED DEMONSTRATION | PUBLIC CORE | Не | Не | Да | P1 | CATALOGUED, NOT BUILT |
| 11 | Rules/attitudes/assumptions explorer (без downward-arrow tool) | INTERACTIVE MODEL | PROFESSIONAL REVIEW REQUIRED | Да | Не | Да | P2 | CATALOGUED, NOT BUILT |
| 12 | Schema hierarchy visualization + criteria continuum demo | ACADEMIC ONLY | ACADEMIC CONTEXT ONLY | Да (за какъвто и да е self-guided вариант) | Не | Да | P2 | CATALOGUED, NOT BUILT |
| 13 | Decision balance simulator | SIMULATOR | PUBLIC WITH ADAPTATION | Не | Потенциално | Да | P2 | CATALOGUED, NOT BUILT |
| 13 | Responsibility pie / credit list | SIMULATOR | PUBLIC WITH ADAPTATION | Не | Потенциално | Да | P2 | CATALOGUED, NOT BUILT |
| 13 | Behavioral exposure planner | SIMULATOR | NOT ELIGIBLE FOR SELF-GUIDED SIMULATOR | Да, задължително | Да | Да | Не в MVP | CATALOGUED, NOT BUILT — блокирано без отделен проф. преглед |
| 14 | Maintenance-curve visualization / general planning demo | STATIC VISUALIZATION / GUIDED DEMONSTRATION | PROFESSIONAL REVIEW REQUIRED | Да | Не | Да | P2 | CATALOGUED, NOT BUILT — без relapse-risk predictor |
| 15 | CBT waves timeline / comparison на подходи | ACADEMIC ONLY | ACADEMIC CONTEXT ONLY | Не | Не | Да | P3 | CATALOGUED, NOT BUILT — без CT-R simulator |

**Всички редове по-горе са изцяло локални (без сървърна заявка), без сървърно съхранение на потребителски данни, съгласно ADR-002.** Дизайн договорът за всеки бъдещ симулатор (ясна учебна цел, keyboard/touch support, text fallback, reset, no hidden scoring/diagnosis/AI analysis, mobile layout, unit-testable state, собственически визуален преглед) е формализиран в Сесия 18 инструкцията и се прилага буквално за всяко бъдещо изграждане от този каталог.

### Redesign Round 2 — Content-Rich, Sectioned and Simulator-Led (Сесия 19, 2026-08-03)

- **Повод:** собственически визуален отказ на Сесия 18 резултата — недостатъчна информация, липса на разпознаваем симулатор, еднакви generic карти без визуален anchor.
- **Обхват:** Седмица 8 (`Сряд 8` реда в таблицата по-горе) реализацията надградена на място — `CbtChainSimulator` (control panel + live diagram + reset) замества по-простия `CbtModelDiagram` **само на тази страница** (диаграмата остава непроменена на `/kpt`/Модул 2 Урок 1); `InterpretationExample`/`CategorizationCheck` пренаписани визуално (branch diagram/workspace), не заменени с нови компоненти. `/kurs` получи course map + week timeline вместо плосък списък. Останалите 14 реда в таблицата по-горе остават `CATALOGUED, NOT BUILT`, непроменени.
- **Acceptance criteria:** build 0/0; 107/107 теста; 8 anchor-навигируеми секции на Седмица 8; симулаторът реално prerender-ва всички опции; branch diagram показва и двата пътя едновременно; comparison matrix и concept map присъстват; `.workspace` контейнер дава реална допълнителна ширина за диаграмите спрямо тесния `.content` четивен стълб.
- **Статус (2026-08-03, Сесия 19): `CONTENT-RICH SIMULATOR REDESIGN READY — OWNER VISUAL REVIEW REQUIRED`.** Некомитнато, трети натрупан checkpoint. Пълни детайли в `10_SESSION_LOG.md` (Сесия 19), вкл. точния pre-redesign визуален одит.
- **Следваща стъпка:** собственически визуален преглед на трите натрупани checkpoint-а заедно → обратна връзка → корекции → един commit.

### Redesign Round 3 — Two-Column Workspace and Semantic Color (Сесия 20, 2026-08-03)

- **Повод:** трети собственически визуален отказ — работната зона зад sidebar-а остава на практика едноколонна, недостатъчна цветова диференциация между типовете съдържание.
- **Обхват:** формализиран `.learning-grid` layout foundation (4 съотношения, 1 breakpoint) и 6-ролева семантична цветова система в `app.css` — и двете реюзируеми за всяка бъдеща седмица/страница, не еднократни за Седмица 8. Седмица 8 и `/kurs` преструктурирани върху тази основа; `CbtChainSimulator` вече показва controls и live output едновременно на desktop. `--container-max` увеличен от 1120px на 1400px.
- **Acceptance criteria:** build 0/0; 120/120 теста; ≥4 `.learning-grid` реда на Седмица 8; controls-преди-output ред в симулатора запазен в DOM; Пълно обяснение + Академичен контекст в общ ред без нов anchor; `/kurs` timeline+sidebar split; всичките 6 accent роли реално сервирани и в двете теми; single-column под 900px потвърдено чрез действителния CSS media block, не низово съвпадение.
- **Статус (2026-08-03, Сесия 20): `TWO-COLUMN COLOR-RICH WORKSPACE READY — OWNER VISUAL REVIEW REQUIRED`.** Некомитнато, четвърти натрупан checkpoint. Пълни детайли, вкл. точните WCAG изчисления за виолет/синьо/индиго, в `10_SESSION_LOG.md` (Сесия 20).
- **Следваща стъпка:** собственически визуален преглед на четирите натрупани checkpoint-а заедно → обратна връзка → корекции → един commit.

### Redesign Round 4 — Global Two-Column Redesign Across All Existing Routes (Сесия 21, 2026-08-04)

- **Повод:** четвърти собственически визуален отказ, с реален screenshot — двуколонният pattern от Redesign Round 3 беше приложен само локално (Седмица 8 + Kurs); открит и коригиран и реален root-cause CSS bug (`.content` без `margin: auto`, залепено вляво в разширения 1400px контейнер).
- **Обхват:** `.learning-grid` breakpoint мина от viewport `@media` на `@container` (sidebar-aware ширина); нов `LearningSection.razor` reusable wrapper; 2 нови бутонни роли (`.btn-blue`/`.btn-neutral`, вече 5 общо); двуколонният workspace pattern приложен ГЛОБАЛНО на всичките 11 реални routes — Home (пълна преработка), Programa, Kpt (пренаписана въпреки предишен `GOOD — KEEP` одит), Модул 1/Модул 2 overview (нов module-overview шаблон), и четирите реални урока (всеки с конкретен нов visual anchor, калибрирано до 3 реда вместо пълните 4 от инструкцията заради по-краткото им реално съдържание — без изкуствено раздуване/нови клинични твърдения).
- **Acceptance criteria:** build 0/0; 159/159 теста; всичките 11 routes показват реален `.learning-grid` usage (не само CSS class presence — преброен реален count на всеки route); `.content` центриран (margin-inline:auto потвърден); container query потвърдена в served CSS; ≥4 бутонни роли реално използвани (5 постигнати); всеки route запазва disclaimer видим; h1=1 навсякъде.
- **Статус (2026-08-04, Сесия 21): `GLOBAL TWO-COLUMN LEARNING WORKSPACE READY — OWNER VISUAL REVIEW REQUIRED`.** Некомитнато, пети натрупан checkpoint. Пълни детайли, вкл. route-by-route breakdown и root-cause bug анализ, в `10_SESSION_LOG.md` (Сесия 21).
- **Следваща стъпка:** собственически визуален преглед на петте натрупани checkpoint-а заедно → обратна връзка → корекции → един commit.

### Final Visual Polish (Сесия 22, 2026-08-04)

- **Повод:** собственикът потвърди глобалната архитектура от Redesign Round 4 като правилна посока; поискан само финален polish — sidebar active-nav bug, Home CTA/learning-path visual, per-lesson progressive disclosure, подобрени visual anchors, workspace gutters, по-хладна dark палитра.
- **Обхват:** реален sidebar bug fix (`NavLinkMatch.All` на всичките 6 линка — предишните 5 разчитаха на подразбиращия се `Prefix` matching, което правеше родителя и текущата цел active едновременно на вложени routes); Home секции; всичките 4 реални урока получиха progressive disclosure + подобрени visual anchors; `.page-container` responsive gutter; navy-charcoal dark палитра (реално WCAG преизчислена); top bar subtitle.
- **Acceptance criteria:** build 0/0; 185/185 теста; точно 1 active sidebar елемент на всеки основен route (не 0, не 2+); Home CTA-та визуално и структурно различими (violet fill vs. blue outline); learning path е реална диаграма с connectors, не текстови кутии; и четирите урока показват progressive disclosure с основната дефиниция извън disclosure; disclaimer никога вътре в disclosure; dark background тон реално различен от предходния (`#0E1420` vs `#1E1D1B`), light темата непроменена.
- **Статус (2026-08-04, Сесия 22): `FINAL VISUAL POLISH READY — OWNER APPROVAL REQUIRED`.** Некомитнато, шести натрупан checkpoint. Пълни детайли в `10_SESSION_LOG.md` (Сесия 22).
- **Следваща стъпка:** собственически финален визуален преглед на шестте натрупани checkpoint-а заедно → одобрение (един commit) или последна конкретна обратна връзка.

### Final Layout Defect Correction (Сесия 23, 2026-08-04)

- **Повод:** реален собственически screenshot преглед — 12 конкретни проблема (2 от тях blocking root-cause bugs: hoризонтален overflow, `<h1>` focus рамка), останалите polish-ниво (stretched columns, nested card chrome, слаб module learning path, error-styled disclaimer, 18 duplicate role-label/heading двойки, слаб status text, sidebar wrapping, hero density, празен top bar).
- **Обхват:** `.learning-grid > * { min-width:0; align-self:start }` формализиран като задължителна част от grid контракта; heading focus CSS отделено от control focus CSS; `ModuleCard`/`DisclaimerCallout` премахнаха "card in card"/"warning in warning" chrome; Модул 1/Модул 2 получиха реален `.module-path` + concept map; систематичен duplicate-label audit и корекция в цялото приложение.
- **Acceptance criteria:** build 0/0; 227/227 теста; без page-level horizontal scrollbar; `<h1>` без control-style focus рамка при нормално зареждане; grid columns никога не се разтягат с празно пространство; educational disclaimer вече не изглежда като error; Модул 1/2 имат реален visual learning path (не само зелен правоъгълник); 0 duplicate role-label/heading двойки; всичко потвърдено в реално сервирания HTML/CSS, не само в кода.
- **Статус (2026-08-04, Сесия 23): `FINAL LAYOUT DEFECTS FIXED — OWNER APPROVAL REQUIRED`.** Некомитнато, седми натрупан checkpoint. Пълни детайли, вкл. точния root-cause анализ за overflow/focus бъговете, в `10_SESSION_LOG.md` (Сесия 23).
- **Следваща стъпка:** собственически финален визуален преглед на седемте натрупани checkpoint-а заедно → одобрение (един commit) или последна конкретна обратна връзка.

### Checkpoint chain resolved — Сесия 24 (2026-08-05)

Всичките седем checkpoint-а по-горе (Сесии 17–23) одобрени от собственика и committed в един foundation commit (`feat: add interactive CBT learning portal foundation`) след финален pre-flight (build/test/route smoke test). Виж `02_CURRENT_STATUS.md` за текущото шестточково разграничение на обхвата и `10_SESSION_LOG.md` (Сесия 24) за пълни детайли. Следваща фаза: `CONTENT-DRIVEN TEMPLATE VALIDATION` (не започната автоматично).

### CONTENT-DRIVEN TEMPLATE VALIDATION — Slice 1: Седмица 1 (Сесия 25, 2026-08-05)

- **Повод:** след одобрения foundation commit, собственикът стартира новата фаза с изричен обхват — само Седмица 1, без redesign, без промяна на application shell освен при реален блокиращ проблем, без commit в тази стъпка.
- **Цел:** валидира втори representative-week архетип — "Theory and History" (историческо/академично съдържание, времева линия, causal-diagram интерактивност), различен от Седмица 8-ия "Simulator Workspace" архетип — доказвайки, че Weekly Course Hub архитектурата (ADR-010) реално мащабира, преди да се строят допълнителни седмици.
- **Обхват:** нов route `/kurs/sedmica-1`, 9 anchor-navigable секции (Накратко/Историческа линия/Научен обрат/Промяна във фокуса/Автоматични мисли preview/Протоколи и наръчници/1979 milestone/Проверка/Обобщение+академичен контекст+източници); два нови reusable компонента с **нула нови CSS класове** (`HistoricalTimeline.razor` — reuse на `.week-timeline`; `ResearchTurnStepper.razor` — reuse на `.cbt-diagram`, адаптиран директно от `CbtModelDiagram`); `comparison-matrix--dual` (вече дефиниран, но неизползван преди тази сесия) — приложен за таблицата "по-ранен срещу нов изследователски въпрос"; knowledge check чрез native `<details>/<summary>` (нула нов quiz engine); `DisclaimerCallout` разширен с optional `Text` параметър (backward-compatible, default текст непроменен, всички съществуващи извиквания без параметър работят идентично).
- **Source governance:** `kpt_syllabus.pdf` използван само като curriculum map (тема/последователност), не като цитируем източник; институционално оформление (кредити/катедра/акредитация) изрично изключено. Основен източник — SRC-041 (Judith Beck). Двете дати (1964, 1979) проверени срещу `10_SESSION_LOG.md` GAP-012 (Closed, потвърдени directно от SRC-041) — само 1979 показана като точна година на страницата, съгласно изричната инструкция да не се добавя точност отвъд дадената.
- **Source reconciliation (2 конфликта, документирани, не мълчаливо решени):** (1) syllabus твърдението, че автоматичните мисли са "лишени от обективна валидност" — заменено с вече заключената формулировка от платформата ("могат да бъдат точни, неточни или частично подкрепени от фактите"); (2) психоанализата не е представена като "провалена" школа — историческият преход е описан като промяна на изследователския въпрос/метод, не като "победа" на една школа над друга (единственото появяване на "победена" в страницата е вътре в изрична отрицателна конструкция, потвърдено при content QA).
- **Cross-linking:** Седмица 1 → `/kpt`, Модул 1, Седмица 8, `/kurs`; Модул 1 → Седмица 1 (като разширен, незадължителен исторически урок — не променя ролята на съществуващия `Modul1Lesson1`); `Kurs.razor` start panel вече сочи към двете налични седмици.
- **Acceptance criteria:** build 0/0; 248/248 теста (227 baseline + 21 нови/актуализирани); 12/12 реални routes 200, несъществуващ route 404; exactly 1 interactive island на страницата; 0 ECTS/институционален бранд/диагностично съдържание; 0 нови CSS класа; disclaimer/source references никога скрити зад disclosure.
- **Статус (2026-08-05, Сесия 25): `WEEK 1 CONTENT-DRIVEN VERTICAL SLICE READY — OWNER CONTENT AND VISUAL REVIEW REQUIRED`.** Некомитнато. Пълни детайли в `10_SESSION_LOG.md` (Сесия 25).
- **Следваща стъпка:** собственически content + visual + responsive преглед → одобрение (commit) или конкретна обратна връзка. Седмица 2/3/10 не са започнати.

### CONTENT-DRIVEN TEMPLATE VALIDATION — Owner review correction (Сесия 26, 2026-08-06)

- **Повод:** собственически визуален преглед на `/kurs/sedmica-1` — общият "Theory and History" архетип потвърден като успешен и запазен; поискани само точкови content-driven корекции, без redesign, без нова седмица, без промяна на application shell/цветова система/grid архитектура/Course Hub структура.
- **Ключов резултат от валидацията (изрично поискан за запис):** споделените Course Hub timeline и CBT diagram patterns бяха преизползваеми принципно, но реалното историческо съдържание изискваше компактна timeline плътност и отделен, responsive research-stepper layout. Това е валиден резултат от content-driven фазата, не дизайнерска грешка — точно затова се строи и validate-ва един реален vertical slice, преди да се мащабира към други седмици.
- **Public-content cleanup:** премахнати всички вътрешни development термини от рендерирания HTML (`11_SOURCE_REGISTER.md`, `kpt_syllabus.pdf`, `citation-grade`, uppercase `ACADEMIC/CLINICAL REVIEW PENDING`) — заменени с learner-facing академичен текст и обикновен български review статус изречение. Файловите имена остават само във вътрешния `@* ... *@` Razor коментар (никога не достига браузъра).
- **ResearchTurnStepper layout:** нов локален CSS Grid layout variant (`.research-turn-stepper`, app.css), scoped само за този компонент — `CbtModelDiagram`-овият споделен `.cbt-diagram__steps` flex-wrap остава напълно непроменен. 3 breakpoint-а (mobile-first): база = 1 колона; 640–899px = 3-колонен zigzag (ред1: 1→2, ред2: connector надолу, ред3: 3→4); 900px+ = единичен ред от 4 равноправни grid tracks. DOM/tab ред остава 1,2,3,4 на всяка ширина — само CSS визуалното разположение се променя.
- **Compact timeline:** нов `Compact` bool параметър на `HistoricalTimeline.razor` + `.week-timeline--compact` CSS модификатор (по-малък padding/gap/marker размер). Course Hub-овата собствена timeline (`Kurs.razor`) остава напълно непроменена — не подава `Compact`.
- **Sidebar contextual state:** нов "weak parent context" слой в `MainLayout.razor` (`IsSectionContext(prefix)` helper + `NavigationManager`) — `/kurs/*` и `/programa/*` вече показват слаб marker (border + приглушен текст, без background fill, без bold) на родителския sidebar item, докато точният route пази силния `.active` (NavLink, вграден `aria-current="page"`). Тествано: exact routes продължават да имат точно 1 `.active`, 0 `.is-context`; sub-routes вече имат точно 1 `.is-context`, 0 `.active` — старият "двоен active" бъг (Сесия 22) не е върнат.
- **Title/badge:** `CourseCatalog.cs` Week 1 Title скъсен на "Как се ражда когнитивната терапия" (премахнато повторението на "преход" с подзаглавието). Собствен `Налично` статус pill на страницата заменен с content-format badge "Теория и история" (`week-list__status--format`, нов CSS модификатор, academic accent tokens) — Course Hub-ът продължава да показва availability чрез собствената си, непроменена metadata система.
- **Knowledge-check wording + Section 09:** инструкцията пренаписана на по-директен, все още non-scored текст. Section 09 разбита на три ясно различими `<h3>` подблока (Какво да запомните / Академичен контекст / Източници и следващи стъпки) — disclaimer остава отделен визуален callout между тях, educational variant.
- **Section 01 density:** премахнат дублиращ параграф (повтарял hero въведението) — сведен до 1 основна теза + 4 idea chips + 1 заключително изречение.
- **Тестове:** 10 нови факта в `Week1ContentSliceTests.cs` (title/badge, internal-language absence, public review text, stepper layout markers, compact timeline markers, sidebar context markers, knowledge-check wording, Section 09 subblocks) + нови helper методи (`ReadLayoutComponent`, `ReadHostFile`, `ReadCss`, `ReadPublicMarkup` — последният съзнателно маха водещия Razor коментар преди forbidden-term проверки, за да не блъска легитимни вътрешни бележки). 258/258 общо (248 + 10 нови).
- **Проверено:** build 0/0; `git diff --check` чист; 12/12 routes 200 на прясна инстанция; decoded HTML (Python `html.unescape`, Blazor сервира кирилица като numeric entity references) потвърждава — 0 внтрешни термини, публичен review текст присъства, нов badge текст присъства, трите h3 subheadings присъстват, стар дълъг title отсъства; sidebar context/active комбинации потвърдени на `/kurs`, `/kurs/sedmica-1`, `/programa/modul-1`, lesson route.
- **Резултат:** `WEEK 1 CONTENT AND TEMPLATE CORRECTIONS READY — OWNER APPROVAL REQUIRED`. Некомитнато. Пълни детайли в `10_SESSION_LOG.md` (Сесия 26).
- **Следваща стъпка:** собственически финален преглед на Седмица 1 → одобрение (commit) или последна конкретна обратна връзка.

### Week 1 slice committed — Сесия 27 (2026-08-06)

Собственикът одобри съдържанието и визуалната реализация на Седмица 1. Финален pre-flight (build/test/route smoke test/content pre-flight) премина чисто и е създаден единствен commit `feat: add week 1 CBT history learning slice`. "Theory and History" архетипът е `VALIDATED` — вторият representative-week архетип на Weekly Course Hub-а (ADR-010) реално мащабира, включително валидния находка от Сесия 26 (споделен layout pattern недостатъчен за реално съдържание → локален, scoped variant). Виж `02_CURRENT_STATUS.md` за текущото разграничение на обхвата и `10_SESSION_LOG.md` (Сесия 27) за пълен commit hash и diffstat. Следваща фаза: `CONTENT-DRIVEN TEMPLATE VALIDATION — WEEK 3` (не започната автоматично).

### CONTENT-DRIVEN TEMPLATE VALIDATION — Slice 2: Седмица 3 (Сесия 28, 2026-08-06)

- **Повод:** върху committed-ата Седмица 1 (hash `6b0efe4`), собственикът стартира втория content-driven vertical slice — само Седмица 3, без Седмица 2/4/10, без глобален redesign, без промяна на заключената foundation архитектура, без commit в тази стъпка.
- **Цел:** валидира трети representative-week архетип — "Concept and Diagram" (йерархични модели, концептуални карти, връзки между нива, сравнения, визуални метафори, ограничена образователна интерактивност), различен и от Седмица 1-ия "Theory and History", и от Седмица 8-ия "Simulator Workspace" — доказвайки, че Weekly Course Hub архитектурата (ADR-010) мащабира и към йерархично/диаграмно съдържание.
- **Обхват:** нов route `/kurs/sedmica-3`, 10 anchor-navigable секции (Три нива/Изследвайте нивата/От ситуация до значение/Когнитивна триада/Схема като филтър/Обработка на информация/Как се свързват нивата/Чести обърквания/Проверка/Обобщение+академичен контекст+източници); два нови interactive компонента — `CognitiveHierarchyExplorer.razor` (wholesale reuse на `.cbt-diagram`, адаптиран от `CbtModelDiagram`/`ResearchTurnStepper`, **0 нови CSS класа**) и `SchemaFilterDemonstration.razor` (единственият реално нов, малък `.schema-filter*` CSS блок тази сесия — toggle+list UI, който не съществуваше преди, оправдан от реална content нужда, не generic framework). Останалите визуализации (три нива йерархия, когнитивна триада, situation-to-meaning, integrated cognitive map) изцяло reuse-ват вече доказани static patterns (`.learning-path-diagram`, `.concept-map__side-notes`, `.concept-map__flow`) — 0 нови компонента за тях. Categorization check и knowledge check reuse-ват native `<details>/<summary>` (Седмица 1 pattern), не нов quiz engine.
- **Source governance:** `kpt_syllabus.pdf` използван само като curriculum map. Основен източник — SRC-041 (Judith Beck, Ch. 3). Когнитивната триада допълнително потвърдена от множество независими регистрирани източници (`11_SOURCE_REGISTER.md` SRC-025/035/037), не само по syllabus-а.
- **Source reconciliation (4 конфликта, документирани, не мълчаливо решени):** (1) основните вярвания не са задължително отрицателни/патологични; (2) "схема като филтър" изрично обозначена като учебна метафора, не буквален механизъм; (3) "автоматично" не се приравнява на "ирационално"; (4) рефлексивната обработка изрично не гарантира безгрешен резултат.
- **Cross-linking:** Седмица 3 → Модул 2, уроците за автоматичните мисли и емоциите, Седмица 1, Седмица 8, `/kurs`; Модул 2 → Седмица 3 разширен в съществуващата "Представителни седмици" sidebar карта, без дублиране на текст. Потвърдено — без линк към несъществуваща Седмица 4.
- **Acceptance criteria:** build 0/0; 285/285 теста (259 baseline + 26 нови/актуализирани); 13/13 реални routes 200, несъществуващ route 404; exactly 2 interactive island-а на страницата; 0 ECTS/институционален бранд/диагностично съдържание/клинични скали; 0 самодиагностика/терапевтични инструкции; disclaimer/source references никога скрити зад disclosure.
- **Статус (2026-08-06, Сесия 28): `WEEK 3 CONCEPT-AND-DIAGRAM VERTICAL SLICE READY — OWNER CONTENT AND VISUAL REVIEW REQUIRED`.** Некомитнато. Пълни детайли в `10_SESSION_LOG.md` (Сесия 28).
- **Следваща стъпка:** собственически content + visual + responsive преглед → одобрение (commit) или конкретна обратна връзка. Седмица 2/4/10 не са започнати.

### Owner review correction — Anchor Navigation and Grid Gap (Сесия 29, 2026-08-06)

- **Повод:** реален собственически преглед на живо докладва 2 blocking дефекта: section-nav anchors отварят Home вместо да скролват на място; height-mismatch между "Чести обърквания" (08) и "Проверка на разбирането" (09) в общ grid ред оставя голяма празна зона.
- **Root cause #1 (потвърден, споделен, не Sedmica3-специфичен):** `App.razor`-овият `<base href="/">` кара всеки bare `href="#id"` в документа да резолва спрямо "/", не спрямо текущата страница — стандартно HTML `<base>`-резолюция поведение, не Blazor бъг. Идентичен pattern потвърден в committed-натите `Sedmica1.razor`/`Sedmica8.razor` и в site-wide skip-link-а в `MainLayout.razor`. Поправено само в Седмица 3 (изрично поискан обхват — пълни пътища в hrefs); другите засегнати места **докладвани, не поправени**, изискват отделно собственическо решение (виж `02_CURRENT_STATUS.md` "Активни проблеми").
- **Root cause #2:** секции с несъответстваща височина, сдвоени в общ `.learning-grid--balanced` ред — CSS Grid оразмерява реда спрямо най-високия елемент. Поправено чрез разделяне в собствени пълноширочинни секции; 08-те 3 карти преминаха на `.concept-map__side-notes` (auto-fit) вместо fixed 2-col grid.
- **Резултат:** двата blocking дефекта коригирани в Седмица 3. 2 нови regression теста (287/287 общо). Некомитнато.

### Systemic Route-Safe Anchor Fix — Week 1, Week 3, Week 8, Global Skip Link (Сесия 30, 2026-08-06)

- **Повод:** довършва анализа от Сесия 29 — bare-fragment anchor бъгът е потвърден системен, не Sedmica3-специфичен, засягайки вече committed-натите `Sedmica1.razor`/`Sedmica8.razor` и site-wide skip-link-а.
- **Поправено:** `Sedmica1.razor` (9 anchors) и `Sedmica8.razor` (8 anchors) вече route-safe, идентично на Седмица 3-ата поправка. Global skip-link в `MainLayout.razor` — динамична поправка (`NavigationManager` + `{uri.AbsolutePath}{uri.Query}#main-content}`), тъй като целта се променя на всеки route. `<base href="/">` в `App.razor` — недокоснат, задължителен.
- **Проверено изчерпателно:** whole-`Components/`-tree regex тест потвърждава 0 останали bare fragment hrefs навсякъде в приложението, не само в познатите файлове — предпазва от бъдеща регресия при нови седмици.
- **Резултат:** anchor-navigation рискът технически разрешен навсякъде, където беше потвърден. 9 нови/актуализирани теста (296/296 общо). Некомитнато — засяга вече committed файлове, чака собственическо одобрение.

### Week 3 slice + systemic anchor fix committed — Сесия 31 (2026-08-06)

Собственикът одобри Седмица 3, anchor навигацията на Седмици 1/3/8, глобалния skip-link и поправения section flow. Финален pre-flight (build/test/route smoke test/anchor navigation pre-flight/content pre-flight/layout pre-flight) премина чисто и е създаден единствен commit `feat: add week 3 cognitive model slice and route-safe anchors`. "Concept and Diagram" архетипът е `VALIDATED` — третият representative-week архетип на Weekly Course Hub-а (ADR-010) реално мащабира. Systemic route-safe anchor contract-ът е `COMPLETE` — bare-fragment риска, потвърден в Седмица 1/3/8 и site-wide skip-link, вече е технически разрешен и в git history. Виж `02_CURRENT_STATUS.md` за текущото разграничение на обхвата и `10_SESSION_LOG.md` (Сесия 31) за пълен commit hash и diffstat. Следваща фаза: `CONTENT-DRIVEN TEMPLATE VALIDATION — WEEK 10` (не започната автоматично).

### CONTENT-DRIVEN TEMPLATE VALIDATION — Slice 3: Седмица 10 (Сесия 32, 2026-08-06)

- **Повод:** върху committed-ата Седмица 3 (hash `e5d55d3`), собственикът стартира третия content-driven vertical slice — само Седмица 10, без Седмица 2/4/9/11, без глобален redesign, без промяна на вече валидираните архетипи, без поправка на несвързаните Седмица 3 наблюдения.
- **Цел:** валидира четвърти representative-week архетип — "Guided Practice" (сократически въпроси, насочено, non-scored изследване на мисъл чрез фиксиран guided-dialogue пример), различен от Theory and History/Concept and Diagram/Simulator Workspace — доказвайки, че Weekly Course Hub архитектурата (ADR-010) мащабира и към насочени въпросни упражнения.
- **Обхват:** нов route `/kurs/sedmica-10`, 10 anchor-navigable секции (Какво е изследване/Четири семейства въпроси/Насочен диалог/Въпрос или прикрит съвет/Факти и заключения/Декатастрофизиране/Балансиран отговор/Практическа последователност/Проверка/Обобщение+академичен контекст+източници); един нов interactive компонент — `SocraticDialogueExplorer.razor` (wholesale reuse на `.cbt-diagram`, **0 нови CSS класа**). Останалите визуализации reuse-ват вече доказани static patterns (`.concept-map__flow`, `.concept-map__side-notes`, `.category-compare`, native `<details>/<summary>` за 2 отделни non-scored checks) — 0 нови компонента за тях.
- **Source governance:** `kpt_syllabus.pdf` използван само като curriculum map. Основен източник — SRC-041 (guided discovery, Socratic questioning, collaborative empiricism).
- **Source reconciliation (4 конфликта, документирани, не мълчаливо решени):** (1) сократическият въпрос не е разпит/прикрит съвет/риторика/доказателство, че мисълта е грешна; (2) балансираният отговор не е принудителен позитивизъм; (3) декатастрофизирането не омаловажава реален проблем; (4) алтернативата не отменя първото обяснение само защото е по-приятна.
- **Cross-linking:** Седмица 10 → Седмица 3, Седмица 8, урока за автоматичните мисли, Модул 2, `/kurs`; Седмица 8 → Седмица 10 добавена като кратка следваща стъпка (единственото малко addititive докосване на вече committed файл, изрично поискано, без промяна на simulator логиката).
- **Acceptance criteria:** build 0/0; 321/321 теста (297 baseline + 24 нови/актуализирани); 14/14 реални routes 200, несъществуващ route 404; exactly 1 interactive island на страницата; route-safe anchors от самото начало; 0 ECTS/институционален бранд/диагностично съдържание/клинично-обучителен език; disclaimer/source references никога скрити зад disclosure.
- **Статус (2026-08-06, Сесия 32): `WEEK 10 GUIDED-PRACTICE VERTICAL SLICE READY — OWNER CONTENT AND VISUAL REVIEW REQUIRED`.** Некомитнато. Пълни детайли в `10_SESSION_LOG.md` (Сесия 32).

### WEEK 10 Owner Visual Review — Final Layout Corrections (Сесия 33, 2026-08-06)

- **Повод:** собственикът одобри Slice 3 архетипа като цяло и поиска 3 точкови layout корекции, без ново съдържание, без глобален redesign, без нова седмица, без Course Hub промени.
- **Обхват на корекциите:** (1) `<h1>` фокус рамка — глобален CSS root-cause fix (слято `:focus`/`:focus-visible` правило за `[tabindex="-1"]` heading-и), ретроактивно валиден за Седмица 1/3/8; (2) Section 01 процесна диаграма — нов scoped `.concept-map__flow--process` responsive модификатор, 0 промяна в другите `.concept-map__flow` употреби; (3) Section 10 — премахнат single-child `.learning-grid--balanced` anti-pattern (same defect клас, недокоснат в Седмица 3 по изричен избор в Сесия 29), заменен с пълноширочинна 4-блокова семантична подялба чрез естествено CSS Grid auto-placement.
- **Acceptance criteria:** build 0/0; 328/328 теста (321 baseline + 7 нови); `git diff --check` чист; fresh-server structural QA потвърждава и трите корекции без регресия на Седмица 1/3/8/Course Hub.
- **Статус (2026-08-07, Сесия 33 финал): `WEEK 10 COMMITTED` — "Guided Practice" архетипът `VALIDATED`.** Собственикът одобри окончателно след 5 корекционни pass-а в Сесия 33 (h1 фокус root-cause fix; Section 01 responsive process + arrow placement; Section 10 full-width подялба; Section 08: process rail → 6-`<li>` семантика → 3×2 → финален два-режимен simplified process). Единствен commit `feat: add week 10 guided practice learning slice`. Пълни детайли в `10_SESSION_LOG.md` (Сесия 33, финал).
- **Следваща стъпка:** `OPTIONAL READING SOURCE COMPONENT` (НЕ Library phase) — не е започната автоматично. Седмица 2/4/9/11 не са започнати.

## Фази 2–9 — ниво на детайл

Запазват детайла, вече документиран в `01_MASTER_PLAN.md` (цел, задачи, зависимости, резултат, критерии за приемане, рискове за всяка фаза). Ще бъдат разбити на STEP-ниво детайл (както Фаза 1 по-горе) в началото на всяка съответна фаза, не предварително — за да не се планира в детайл работа, чиито предпоставки (напр. реален MVP UI за Фаза 2 дизайн решенията) все още не съществуват.

## Обобщена зависимост верига

```
Одобрение (MVP + стек + IA + data model + prototype decision)
  → Фаза 1 (STEP-1.1 … 1.6)
    → Фаза 2 (дизайн система)
      → Фаза 3 (съдържателна система, флагмански модул)
        → Фаза 4 (интерактивно упражнение)
          → Фаза 5 (проследяване на прогреса)
            → Фаза 7 (QA преди launch)
              → Фаза 8 (deployment)
```

Фаза 6 (администрация) и Фаза 9 (следващи версии) остават извън тази верига — Deferred, изпълняват се след успешен MVP launch.

---

## Systematic Curriculum Expansion — Architecture Map (Phase A, Сесия 35, 2026-08-08)

**Status: `SYSTEMATIC CURRICULUM EXPANSION MAP READY — OWNER REVIEW REQUIRED`. Planning-only — no new route/component/page/commit in this phase.**

Sources used: `CourseCatalog.cs` (syllabus-derived metadata, kpt_syllabus.pdf reference), the Simulator Opportunity Matrix above (Сесия 18), `11_SOURCE_REGISTER.md` (SRC-041 entry + GAP-006/008/009/010/011), `10_SESSION_LOG.md` chapter-reading narrations (Сесия 3 and continuations — the only chapters treated as source-confirmed are ones with specific narrated content, not the registry's blanket "100% reviewed" claim, matching the precedent already set for Week 8/10's `OptionalReadingSource` integration), `23_CLINICAL_SAFETY_BOUNDARIES.md`, and the four validated archetypes (Week 1/3/8/10). No new curriculum themes invented; no general CBT knowledge substituted for syllabus/source content.

**Chapters treated as source-confirmed (session-log-narrated, not just registry-claimed):** Гл. 1 (история, 10-те принципа, обзор на сесия), Гл. 2 (обзор на лечението), Гл. 3 (когнитивна концептуализация — вече цитирана за Седмица 3/11), Гл. 4 (оценъчна сесия), Гл. 5 (структура на първата сесия, "45–50 минути"), Гл. 6 (поведенческа активация), Гл. 7 (само началото), Гл. 11 (12-те когнитивни изкривявания, Фигура 11.2), Гл. 14 (основни вярвания, 3-те категории). Всичко извън този списък за конкретна седмица е маркирано `CONTENT DETAIL — NEEDS SOURCE VERIFICATION` по-долу, независимо от регистровото твърдение за 100% прочит на книгата.

### 11-Week Matrix

| Седмица | Тема (syllabus) | Primary archetype | Secondary pattern | Interaction? | Визуализации (макс. 3) | Source status | Safety/review | Сложност | Зависимости |
|---|---|---|---|---|---|---|---|---|---|
| **2** | Когнитивна терапия на Бек и REBT на Елис | Theory and History | Concept and Diagram (comparison) | NO — статичен comparison table; timeline reuse от Седмица 1 | Comparison table, extended timeline | Гл. 1 (обща история) confirmed; **GAP-008 open** — конфликт между два comparison frame-а (SRC-001 vs SRC-006), трябва избор/съчетаване с цитат преди писане | PublicCore, без клиничен риск | LOW | Седмица 1 (reuse HistoricalTimeline) |
| **4** | Клинична оценка и когнитивна концептуализация | Concept and Diagram | Theory and History | NO — matrix: ACADEMIC ONLY; забранено въвеждане по дизайн | Process/concept diagram (общи категории, без форма за попълване) | **Гл. 4 confirmed** | AcademicContextOnly; assessment concepts — "как специалистът концептуализира", никога "оцени себе си"; проф. преглед при всеки бъдещ разширен вариант | LOW–MEDIUM | Reuse Седмица 3 concept-map patterns |
| **5** | Принципи на КПТ и терапевтичен съюз | Concept and Diagram | Guided Practice | YES — лек, reuse на Седмица-10 comparison-card pattern (collaborative vs directive) | 10-principles card grid, collaborative-vs-directive comparison | **Гл. 1 confirmed за 10-те принципа**; alliance sub-topic — NEEDS SOURCE VERIFICATION | PublicWithAdaptation, лека клинична чувствителност (alliance framing) | LOW–MEDIUM | Седмица 1 (Гл.1 overlap), Седмица 10 (pattern reuse) |
| **6** | Структура на терапевтичната сесия | Guided Practice | Concept and Diagram | YES — минимален, progressive-disclosure walkthrough (без нов компонент) | Session-stage flow diagram, structure card list | **Гл. 5 confirmed** ("45–50 минути", GAP-010 разрешен) | PublicWithAdaptation; explicit "информационен преглед, не self-therapy instruction" | LOW–MEDIUM | Седмица 5, Седмица 10 (guided-sequence reuse) |
| **7** | Поведенческа активация | Simulator Workspace | Concept and Diagram | YES — САМО fixed-example variant, не личен planner | Behavior-mood cycle diagram, fixed-example comparison cards | **Гл. 6 confirmed** (заглавието съвпада) | PublicWithAdaptation; проф. преглед изискван само за реалния planner (избягван тук) | MEDIUM (нов компонент, reuse на CbtChainSimulator архитектура) | Седмица 8 (simulator pattern) |
| **9** | Когнитивни изкривявания и дневник на мислите | Simulator Workspace | Guided Practice | YES за distortion-matching (reuse CategorizationCheck); NO за thought record — остава статичен worked example, никога fillable форма | 12-distortion card grid, static worked-example "record" table | **Гл. 11 confirmed** (12 изкривявания, Фиг. 11.2, GAP-006/009 разрешени); точната глава за thought-record структура — NEEDS SOURCE VERIFICATION | PublicWithAdaptation; **CLINICAL SAFETY PROFILE — HIGH CARE**: тънка граница safe/unsafe, изисква изрично owner потвърждение на дизайна | MEDIUM–HIGH (най-съдържателна седмица) | Седмица 8, Седмица 10 (твърди prerequisites) |
| **11** | Междинни вярвания | Concept and Diagram | Guided Practice (лек, non-self-application) | YES, LOW-RISK ONLY — static-first; matrix изключва downward-arrow tool | Zoom-in hierarchy diagram (extend CognitiveHierarchyExplorer), recurring-pattern hub diagram | **Гл. 3 confirmed за дефиницията**; под-типовете (rules/attitudes/assumptions) — NEEDS SOURCE VERIFICATION | `ProfessionalReviewRequired`; граничи със self-assessment — трето лице framing задължително | MEDIUM–HIGH | Седмица 3 (hard prerequisite), полза от Седмица 9 |
| **12** | Основни вярвания и схеми | Concept and Diagram | Theory and History (academic framing) | NO — matches AcademicOnly | Deepest-layer hierarchy diagram, 3-category card grid | **Гл. 14 confirmed** — най-силна источникова седмица от 11-те | AcademicContextOnly; строго трето лице | LOW–MEDIUM | Седмица 3, Седмица 11 (build след 11) |
| **13** | Вземане на решения и поведенчески техники | Concept and Diagram | Simulator Workspace (scoped само до decision-balance/responsibility-pie) | YES само за decision-balance/responsibility-pie (fixed-example); NO абсолютно за exposure planner | Decision-balance concept diagram, responsibility-pie concept diagram, graduated-exposure static staircase (concept only) | **Няма потвърдена глава за никоя под-тема** — най-слабо източникова седмица | `NotEligibleForSelfGuidedSimulator` — най-строгото ниво в системата; **MAXIMUM CARE**; exposure под-темата изисква отделно owner решение | HIGH (смесени safety нива в една седмица) | Седмица 7, Седмица 8 |
| **14** | Домашна работа, приключване и превенция на рецидив | Concept and Diagram | Guided Practice (лек) | NO — matches StaticVisualization; изключва relapse-risk predictor | Illustrative maintenance-curve (non-predictive), structured plan-elements card list | Не потвърдена конкретна глава — NEEDS SOURCE VERIFICATION | `ProfessionalReviewRequired`; никога personal risk assessment | MEDIUM | Седмица 6 (narrative closure) |
| **15** | Трета вълна и възстановително-ориентирана терапия | Theory and History | Concept and Diagram (approach comparison) | NO — matches AcademicOnly; изключва CT-R simulator | Extended historical timeline (reuse HistoricalTimeline), approach-comparison cards | **GAP-011 open** — конфликт в годините на "трите вълни"; SRC-041 (2011) вероятно мълчи по CT-R терминология → OptionalReadingSource може да изисква друг източник | AcademicContextOnly, ниска клинична чувствителност | LOW | Седмица 1/2 (timeline continuation) |

### C. Duplication Audit (Week 8/9/10/11/12)

- **Седмица 8** (built): модел Ситуация→Мисъл→Емоция→Поведение; разграничение мисъл/емоция (CategorizationCheck); interpretation branching.
- **Седмица 9** (planned): 12-те когнитивни изкривявания (НОВО); структурата на thought record като илюстративен worked example (НОВО); изрично надгражда разграничението от Седмица 8, не го преповтаря.
- **Седмица 10** (built): Сократически въпроси; факт vs предположение vs заключение; декатастрофизиране; балансиран отговор; алтернативни обяснения.
- **Седмица 11** (planned): междинни вярвания специфично (rules/attitudes/assumptions) — задълбочава йерархията от Седмица 3, нов ъгъл — повтарящ се патерн от автоматични мисли (не единична мисъл) → underlying belief.
- **Седмица 12** (planned): основни вярвания/схеми специфично — най-дълбокото ниво, надгражда Седмица 11.

**Заключение:** без реално дублиране, при условие че Седмица 9 остане строго за изкривяванията+record-структурата, а Седмица 11/12 останат строго за съответното ниво на йерархията.

### D. Clinical Safety Map (Week 4–7 + 9/11/13/14)

| Седмица | Assessment concepts | Терапевтичен съюз | Session structure | Behavioral activation | Case formulation | Symptom/risk references | Ниво на грижа |
|---|---|---|---|---|---|---|---|
| 4 | Да (концептуално, трето лице) | Не | Не | Не | Да (обзорно) | Не | HIGH — assessment framing |
| 5 | Не | Да | Не | Не | Не | Не | LOW–MEDIUM |
| 6 | Не | Не | Да | Не | Не | Не | MEDIUM — "не self-therapy instruction" |
| 7 | Не | Не | Не | Да (fixed-example само) | Не | Не | MEDIUM — personal-data риск ако не остане fixed-example |
| 9 | Не | Не | Не | Не | Не | Не (само labels) | HIGH — тънка граница safe/unsafe за thought-record |
| 11 | Граничи (self-identification риск) | Не | Не | Не | Не | Не | HIGH — ProfessionalReviewRequired |
| 13 | Не | Не | Не | Да (decision/exposure) | Не | Потенциално (exposure) | MAXIMUM — NotEligibleForSelfGuidedSimulator |
| 14 | Не | Не | Не | Не | Не | Граничи (relapse) | HIGH — ProfessionalReviewRequired |

Забранени за всички изброени: self-diagnostic tools, suicide-risk tools, clinical screening questionnaires, personal symptom scoring, self-treatment protocols, free-text patient cases. Клиничните процедури се представят като "как специалистът концептуализира/структурира", никога "направете това върху себе си" (per `23_CLINICAL_SAFETY_BOUNDARIES.md`).

### E. Recommended Implementation Order (1→11)

1. Седмица 2 — LOW, reuse-heavy, продължава Седмица 1 директно.
2. Седмица 5 — LOW–MEDIUM, силен източник (Гл. 1), reuse на Седмица-10 patterns.
3. Седмица 6 — LOW–MEDIUM, силен източник (Гл. 5), логично след 5.
4. Седмица 4 — по-висока safety чувствителност — построена след 3 доказани ниско-рискови седмици.
5. Седмица 7 — MEDIUM, първи нов interactive компонент в тази фаза.
6. Седмица 9 — MEDIUM–HIGH, изисква 8 и 10 вече готови (удовлетворено).
7. Седмица 11 — MEDIUM–HIGH, ProfessionalReviewRequired, изисква Седмица 3.
8. Седмица 12 — LOW–MEDIUM, най-силен източник, логично след 11.
9. Седмица 15 — LOW, поставена след cognitive-model батча заради GAP-011 нужда.
10. Седмица 13 — HIGH, най-риск-скопирана и най-слабо източникова — построена след доказаните по-леки седмици.
11. Седмица 14 — MEDIUM, ProfessionalReviewRequired, логично затваря целия curriculum narrative — построена последна.

### F. Batch Plan

**BATCH A — Foundation Continuity** (Седмица 2, 5, 6). Rationale: разширяват вече валидирани архетипи с минимум нови компоненти; завършват сравнителната част на Модул 1 + по-голямата, безопасна част на Модул 2. Dependencies: Седмица 1, Седмица 10. Review gate: стандартен content review.

**BATCH B — Clinical-Adjacent Process** (Седмица 4, 7). Rationale: и двете засягат clinician-perspective процес, изискващи внимателно "как специалистът, не как ти" framing — групирани за един clinical-safety review pass. Dependencies: Batch A, Седмица 8. Review gate: задължителен `23_CLINICAL_SAFETY_BOUNDARIES.md` §8 преглед.

**BATCH C — Beliefs & Distortions Deepening** (Седмица 9, 11, 12). Rationale: надграждат една и съща йерархия, носят най-висок duplication риск плюс ProfessionalReviewRequired (11) — групирани за единен reviewer pass. Dependencies: Седмица 3, 8, 10. Review gate: задължителен професионален преглед за Седмица 11; препоръчан за целия батч.

**BATCH D — Final Integration & Restricted Topics** (Седмица 15, 13, 14). Rationale: Седмица 15 затваря историческата линия; Седмица 13 носи най-строгото safety ниво и вероятно се нуждае от РАЗДЕЛЕН преглед (безопасна decision-tools част срещу блокираната exposure част); Седмица 14 затваря curriculum narrative-а. Dependencies: практически цялото предходно съдържание. Review gate: задължителен професионален преглед за 13/14; exposure под-обхватът на 13 изисква отделно, изрично owner решение.

### G. New Archetype Candidates

**NONE.** Всичките 11 оставащи седмици се map-ват удобно върху четирите вече валидирани архетипа (понякога primary+secondary) — не е установена фундаментална учебна нужда, непокрита от Theory and History / Concept and Diagram / Simulator Workspace / Guided Practice.

### H. Source Gaps — Consolidated (изисква прочит/потвърждение преди coding)

- **Седмица 2:** GAP-008 (REBT-vs-CBT frame конфликт, open).
- **Седмица 5:** потвърждение за "терапевтичен съюз" sub-topic отвъд общото Гл.1 покритие.
- **Седмица 9:** точната глава/фигура за структурата на thought record.
- **Седмица 11:** под-класификацията на междинни вярвания (rules/attitudes/assumptions).
- **Седмица 13:** цялото съдържание — decision balance, responsibility pie, graduated exposure — без потвърдена глава.
- **Седмица 14:** глава за домашна работа/приключване/превенция на рецидив.
- **Седмица 15:** GAP-011 (годините на "трите вълни") + дали SRC-041 изобщо покрива "трета вълна"/CT-R.

### I. Project OS changes (тази стъпка)

- `24_IMPLEMENTATION_ROADMAP.md` — добавен този раздел (Architecture Map, Phase A).
- `02_CURRENT_STATUS.md` — текущата стъпка отбелязана като `SYSTEMATIC CURRICULUM EXPANSION — PLANNING`.
- Без нов Project OS файл, без нов ADR, без промяна на `04_CHANGELOG.md` (планиране само, не имплементиран feature).

### J. Status

`SYSTEMATIC CURRICULUM EXPANSION MAP READY — OWNER REVIEW REQUIRED`

---

## Systematic Curriculum Expansion — Source Gap Closure + Build Order Freeze (Phase B, Сесия 35 продължение, 2026-08-09)

**Status: `SOURCE GAPS REVIEWED — IMPLEMENTATION ORDER READY FOR OWNER APPROVAL`. Planning-only — no new route/component/page/commit in this phase.** Rule applied throughout: "FULLY REVIEWED" в `11_SOURCE_REGISTER.md` е регистров статус за цялата книга, не автоматично доказателство за конкретна глава/теза за дадена седмица — само сесия-лог наративи с конкретно съдържание се третират като confirmed (същият стандарт от Phase A).

### A. GAP-008 / Week 2

- **Status:** `CONSTRAINED` (не BLOCKING, не пълно RESOLVED — няма единствена "правилна" рамка за REBT-vs-CBT сравнение, но темата е безопасно ограничима).
- **Allowed claims (cross-source потвърдени):** Елис разработва REBT през 1950-те, преди Бек да разработи CBT през 1960-те (SRC-023); REBT често се класифицира като форма/предшественик на по-широкото CBT семейство (SRC-011); двата подхода споделят фокус върху промяна на неполезни мисловни модели; академичен източник (SRC-030, Cambridge Core) изрично препоръчва съчетаване на двете рамки, не избор на "правилната".
- **Claims requiring qualification:** точните conception years отвъд декадно ниво (1950-те/1960-те) — не се цитират конкретни години без директна source опора за самата година; изборът на кои "измерения на сравнение" да се представят изисква editorial избор в началото на реалното писане, информиран от 3-те налични вторични рамки (SRC-006 философска, SRC-030 академична, SRC-011 практическа), не механично копиране на нито една.
- **Claims excluded:** прототипната 4-измерна рамка (централен термин/патогенеза/стил/тестване) — непотвърдена от нито един прегледан вторичен източник; всякакво "една школа заменя/побеждава другата" framing; третиране на REBT и Beck CBT като идентични.
- **Confirmed sources:** SRC-006, SRC-011, SRC-012, SRC-023, SRC-030 (вторични, cross-checked, ползвани по изключението "SRC-041 мълчи по темата", `23_CLINICAL_SAFETY_BOUNDARIES.md` §9).
- **Unresolved items:** точната comparison-dimension структура за реалния текст — editorial решение при писане, не архитектурен blocker.
- **Статус:** `READY`.

### B. Week 5 source status

- **Потвърдено:** Гл. 1 явно съдържа "10-те принципа" (сесия-лог, дословен запис). "Collaborative empiricism" вече е установен, source-backed термин в проекта (потвърден и цитиран за Седмица 10) — пряко релевантен академичен анкор за "терапевтичен съюз/сътрудничество" темата на Седмица 5, макар и без собствен отделен глава-номер locator.
- **Непотвърдено:** точна страница/фигура специфично за думата "alliance"/"съюз" — не е отделно наративно потвърдена в session log, отвъд общото Гл.1 покритие.
- **WEEK 5 CONTENT CONTRACT:** 10-те принципа — цитируеми директно от Гл. 1; "сътрудничество" се представя чрез установения термин collaborative empiricism (не нов, непотвърден claim), без специфични твърдения, изискващи отделен locator.
- **Статус:** `READY` (не PARTIALLY — Гл. 1 покрива основното съдържание; softness-ът е само в точния citation locator за една дума, не структурен риск).

### C. Week 9 safe content contract

- **SAFE EDUCATIONAL CONTENT (позволено):** 12-те когнитивни изкривявания като etiketi + fixed илюстративни примери (Гл. 11, Фиг. 11.2, потвърдено); "anatomy of a record" — статична, worked-example структура показваща какви полета съществуват и как се свързват (ситуация/мисъл/доказателство/алтернатива), **без потребителски вход никъде**; distortion-matching упражнение върху fixed примери (reuse на `CategorizationCheck` pattern).
- **PERSONAL THERAPEUTIC WORKSHEET BEHAVIOR (изрично изключено):** personal free-text thought record; symptom scoring; съхранени лични записи (localStorage/IndexedDB); therapy-like self-guidance ("попълни своята мисъл тук"). Session log потвърждава multi-session standing prohibition ("не се стартира thought record упражнение") — повтарящо се изрично изключение в 9+ отделни сесии, най-силният сигнал в целия проект за extra care на тази граница.
- **Разграничение от Седмица 8/10:** Седмица 9 не преповтаря мисъл/емоция разграничението (8) нито факт/заключение метода (10) — добавя единствено НОВАТА distortion-таксономия + илюстративната record-структура.
- **Source status:** Гл. 11 confirmed (изкривяванията); точната глава/фигура за record-структурата остава `NEEDS SOURCE VERIFICATION`.
- **Статус:** `PARTIALLY READY` — coding не започва без изрично owner потвърждение на "static worked-example only" дизайн решението, дори след source verification.

### D. Week 11 belief-source contract

- **Потвърдено:** Гл. 3 ("The Cognitive Model") е потвърден основен източник за йерархията автоматична мисъл→междинно вярване→основно вярване (сесия-лог, вече цитирано и за Седмица 3). Модул 2 (Урок 2) съзнателно пропусна междинни/основни вярвания precisely за да запази обхвата за бъдещата Седмица 11/12 (документирано решение, Сесия 19) — потвърждава, че тази седмица е genuinely новото място за темата, не дублиране.
- **Непотвърдено:** конкретната под-класификация на междинни вярвания (rules/attitudes/assumptions като 3 различни под-типа) — не е source-confirmed от session log; **не се създава taxonomy от памет**.
- **WEEK 11 CONTENT CONTRACT:** дефиниция на междинно вярване + връзка с повтарящ се патерн от автоматични мисли — цитируеми от Гл. 3; под-типовете (ако въобще се включат) чакат отделна source-верификация; downward-arrow tool и self-application framing остават изрично изключени (matrix).
- **Professional review:** остава `REQUIRED` (CourseCatalog flag, непроменено).
- **Статус:** `PARTIALLY READY`.

### E. Week 13 strict safety contract

**Curriculum rule заключена:** `NotEligibleForSelfGuidedSimulator` — най-строгото ниво в системата, прилага се безусловно.

- **A. SAFE EDUCATIONAL / DECISION-TOOL CONTENT (INCLUDED):** decision-balance концептуална диаграма (fixed example); responsibility-pie концептуална диаграма (fixed example); graduated-exposure ПРИНЦИП като статична стълба/continuum диаграма (обяснение на идеята, не инструмент).
- **B. PROFESSIONAL-PROCEDURE CONTENT (EXCLUDED от тази фаза):** self-directed exposure procedure; personal risk/task generator; graded exposure planner; free-text personal experiment builder — **нито едно от тези не се имплементира** без отделен професионален преглед.
- **REQUIRES PROFESSIONAL REVIEW:** цялата exposure под-тема, дори в чисто концептуалната си форма, преди публикуване.
- **Source status:** `NEEDS SOURCE VERIFICATION` — няма потвърдена глава за никоя под-тема (decision balance, responsibility pie, или exposure principle). Най-слабо източникова седмица от всичките 11.
- **Статус:** `NOT READY` — изисква source-reading pass ПРЕДИ coding да започне, plus отделно owner go/no-go за exposure под-обхвата.

### F. Week 14 source status

- **Потвърдено:** нищо конкретно — темата (домашна работа, приключване, превенция на рецидив) не се появява в нито един narrated session-log reading pass.
- **Curriculum-only:** темата съществува в CourseCatalog (syllabus-derived), но без source locator.
- **Изисква:** нов reading pass на SRC-041 (вероятно късни глави, извън вече прочетените 1,2,3,4,5,6,7-начало,11,14) преди content contract да може да се финализира. Не се измисля глава номер.
- **WEEK 14 CONTENT CONTRACT:** не може да бъде изведен пълноценно преди source verification; предварителна safety рамка (без relapse-risk predictor, без personal plan builder) остава в сила от Phase A.
- **Статус:** `NOT READY`.

### G. GAP-011 / Week 15

- **GAP-011 статус:** `CONSTRAINED` за "вълновото" рамкиране (3 независими прегледани източника — SRC-014, SRC-023, SRC-025 — потвърждават самия модел; нито един не съвпада с прототипните точни години → правило: никога точните прототипни години, само диапазони съвместими с прегледаните източници, или пропускане на спорни точни години).
- **SOURCE-CONFIRMED:** съществуването на "вълново" рамкиране на КПТ традицията (3 независими вторични източника).
- **SYLLABUS-DERIVED:** заглавието "Трета вълна и възстановително-ориентирана терапия" (CourseCatalog).
- **NEEDS ADDITIONAL SOURCE:** "възстановително-ориентирана терапия"/CT-R терминологията — не потвърдена от НИКОЙ прегледан източник; SRC-041 (2011, 2-ро изд.) хронологично малко вероятно да покрива тази по-късна терминология — **не се приписва на SRC-041**.
- **Статус:** `CONSTRAINED` (вълни) / `OPEN — needs additional source` (CT-R под-тема, скопирана отделно, не блокира вълновата половина).

### H. Weeks 4/6/7/12 verification

| Седмица | SOURCE READY | Основание |
|---|---|---|
| 4 | **YES** | Гл. 4 ("Оценъчна сесия") наративно потвърдена, session log |
| 6 | **YES** | Гл. 5 потвърдена дословно ("45–50 минути"), GAP-010 вече Closed |
| 7 | **YES** | Гл. 6 ("Поведенческа активация") — заглавието директно съвпада, потвърдена |
| 12 | **YES** | Гл. 14 потвърдена с точно съдържание (3-те категории), най-силна от всичките 11 |

Няма нов gap, открит за тези четири седмици.

### I. Remaining blocking gaps

**Няма пълно WEEK-LEVEL blocking gap** след constraint-ването на GAP-008 и GAP-011. Остават обаче два `NOT READY` статуса (Седмица 13, Седмица 14), изискващи source-reading pass ПРЕДИ coding да започне за тях конкретно — това е pre-coding gate за тези две седмици, не blocker за целия build order (те просто седят последни в новия ред по-долу).

### J. Final implementation order (заменя реда от Phase A — не запазен механично)

1. **Седмица 6** — най-чист source match (дословен цитат), reuse на доказан Guided Practice archetype, нисък архитектурен риск.
2. **Седмица 12** — най-силен източник от всичките 11 (Гл. 14), AcademicOnly, нула нова интерактивност.
3. **Седмица 7** — потвърден източник (Гл. 6), нов компонент но reuse на доказана Simulator архитектура (Седмица 8).
4. **Седмица 4** — потвърден източник (Гл. 4), по-висока safety чувствителност — построена след 3 доказани седмици.
5. **Седмица 5** — READY, лек interaction, reuse на Седмица-10 patterns.
6. **Седмица 2** — READY, но изисква cross-source synthesis (GAP-008 constrained) — editorial-тежка, не source-тежка.
7. **Седмица 9** — PARTIALLY READY, изисква Седмица 8+10 (удовлетворено); построена след повече "reps" заради тънката safe/unsafe граница.
8. **Седмица 11** — PARTIALLY READY, ProfessionalReviewRequired, полза от Седмица 9-ия pattern concept (изрично поискана зависимост).
9. **Седмица 15** — CONSTRAINED (вълни), CT-R под-тема скопирана отделно; естествено затваря историческата линия.
10. **Седмица 13** — NOT READY, най-строг safety tier — построена след всички по-леки седмици доказани.
11. **Седмица 14** — NOT READY, ProfessionalReviewRequired, логично затваря целия curriculum narrative.

*Забележка:* позиции 7–8 (Седмица 9, 11) са две последователни MEDIUM–HIGH сложност седмици — умишлено изключение от строгата low/medium/high редукция, директно защото собственикът изрично поиска "Week 9 преди 11/12, ако съдържателната ѝ функция е prerequisite" (§12) — приоритет на content dependency пред чисто редуване на сложност.

### K. Final batches

**BATCH A — Clean Source, Zero Synthesis** (Седмица 6, 12, 7)
- Why grouped: единствен, чист confirmed chapter за всяка; нула отворени GAP-та; нула editorial synthesis нужда.
- Source readiness: READY (и трите).
- Dependency: Седмица 10 (pattern reuse за 6), Седмица 8 (simulator архитектура за 7).
- Review gate: стандартен content review; без елевиран clinical gate.
- Definition of done: page live, tests green, content-review checklist подписан, без ескалация към проф. преглед.

**BATCH B — Clinical Care + Source Synthesis** (Седмица 4, 5, 2)
- Why grouped: всяка изисква точно един допълнителен editorial pass — Седмица 4 (safety framing "специалист, не ти"), Седмица 5 (alliance-term нюанс), Седмица 2 (GAP-008 cross-source синтез) — различно от Batch A-ото "просто транскрибирай главата".
- Source readiness: READY (и трите, с отбелязани нюанси).
- Dependency: Седмица 1 (timeline reuse за 2), обща Модул 2 последователност.
- Review gate: `23_CLINICAL_SAFETY_BOUNDARIES.md` §8 проверка за Седмица 4; изрично owner sign-off на избраната GAP-008 comparison рамка за Седмица 2.
- Definition of done: page live + tests + (Седмица 4) safety framing документиран преглед + (Седмица 2) GAP-008 статус в `15_GAPS_AND_CONFLICTS.md` актуализиран спрямо реално използваната рамка.

**BATCH C — Belief Hierarchy Deepening, High Care** (Седмица 9, 11)
- Why grouped: и двете носят най-тънката safe/unsafe граница в целия каталог + пряка зависимост от Batch A/B основите (3/8/10).
- Source readiness: PARTIALLY READY (и двете).
- Dependency: Седмица 3, 8, 10 (удовлетворени); Седмица 9 преди Седмица 11 в самия batch.
- Review gate: ЗАДЪЛЖИТЕЛЕН професионален преглед за Седмица 11 (CourseCatalog flag); изрично owner design-confirmation gate за Седмица 9 ПРЕДИ coding, не само преди publish.
- Definition of done: и двете live, tests green, Седмица 11 с документиран проф. преглед, Седмица 9 с автоматизиран тест, потвърждаващ 0 free-text/storage механизми в реалния код.

**BATCH D — Restricted / Weakest-Sourced / Final** (Седмица 15, 13, 14)
- Why grouped: изискват най-много ново четене на SRC-041 преди content да може да се финализира + носят най-рестриктивните safety нива в целия curriculum.
- Source readiness: Седмица 15 CONSTRAINED (вълни)/NOT READY (CT-R); Седмица 13 NOT READY; Седмица 14 NOT READY.
- Dependency: практически целият предходен curriculum narrative.
- Review gate: ЗАДЪЛЖИТЕЛЕН професионален преглед за Седмица 13 и 14; exposure под-обхватът на 13 изисква ОТДЕЛНО, изрично owner решение, независимо от останалата седмица; CT-R под-темата на 15 или се пропуска от MVP обхвата, или изрично чака нов източник.
- Definition of done: Седмица 15 live само с вълновата половина (dates в диапазони); Седмица 13 live само с decision-tools под-обхвата, exposure-планер изрично НЕ построен с документирано owner решение; Седмица 14 live само след нов reading pass, потвърждаващ конкретна глава, plus проф. преглед.

### L. First implementation candidate

**FIRST IMPLEMENTATION CANDIDATE: WEEK 6**

- **Source ready:** Гл. 5 потвърдена дословно ("Повечето стандартни сесии по когнитивна поведенческа терапия траят около 45–50 минути") — най-силният единичен цитат-match извън Седмица 12.
- **Archetype known:** Guided Practice — вече валидиран, доказан архетип (Седмица 10), директно приложим за "структура на сесия" walkthrough.
- **Low architectural risk:** не изисква нов интерактивен компонент — progressive disclosure (native `<details>`) и статични diagram patterns стигат, reuse-heavy.
- **Curriculum dependency appropriate:** логично следва Седмица 5 (принципи преди структура), не блокирана от нищо неготово.

**Не е имплементирана в тази стъпка.**

### M. Optional Reading mapping

| Седмица | Optional Reading статус |
|---|---|
| 2 | **PENDING** — SRC-041 не покрива REBT задълбочено; нужно решение дали да се цитира вторичен източник (напр. SRC-030) вместо обичайния SRC-041 pattern |
| 4 | **CONFIRMED CHAPTER** — Гл. 4 |
| 5 | **CONFIRMED CHAPTER** — Гл. 1 |
| 6 | **CONFIRMED CHAPTER** — Гл. 5 |
| 7 | **CONFIRMED CHAPTER** — Гл. 6 |
| 9 | **CONFIRMED CHAPTER** (Гл. 11, само за изкривяванията) / **PENDING** (record-структура) |
| 11 | **CONFIRMED CHAPTER** (Гл. 3, само за дефиницията) / **PENDING** (под-типове) |
| 12 | **CONFIRMED CHAPTER** — Гл. 14 |
| 13 | **PENDING** — няма потвърдена глава за никоя под-тема |
| 14 | **PENDING** — няма потвърдена глава |
| 15 | **PENDING** (вълни — вероятно вторичен източник, не SRC-041) / **NOT NEEDED от SRC-041** (CT-R — хронологично невъзможно) |

Публичен URL остава отделен, непроменен въпрос — `PUBLIC SOURCE URL — PENDING VERIFICATION` за SRC-041 остава в сила от Сесия 34.

### N. Project OS changes (тази стъпка)

- `24_IMPLEMENTATION_ROADMAP.md` — добавен този раздел (Source Gap Closure + Build Order Freeze, Phase B).
- `15_GAPS_AND_CONFLICTS.md` — GAP-008 и GAP-011 актуализирани от `Open` на `Constrained` статус, с точна обосновка и реф. към новите content contracts.
- `02_CURRENT_STATUS.md` — текущата стъпка отбелязана съответно.
- Без нов Project OS файл, без нов ADR, без промяна на `04_CHANGELOG.md`, без седмица маркирана `COMPLETE`.

### O. Status

`SOURCE GAPS REVIEWED — IMPLEMENTATION ORDER READY FOR OWNER APPROVAL`

---

## Systematic Curriculum Expansion — Phase C: Week 6 Implementation (Сесия 35, 2026-08-09)

First position of the frozen build order (§J above) implemented. Full narrative in `10_SESSION_LOG.md` (Сесия 35). Summary: `Sedmica6.razor` built with zero new components/CSS (reuses `LearningSection`/`ProgressiveExplanation`/`DisclaimerCallout`/`SourceReferences`/`OptionalReadingSource`, `.category-compare`, `.concept-map__side-notes`); source contract held to SRC-041 Гл. 5 exactly (duration quote + general 3-part shape per GAP-010's "close to prototype" resolution) — the original prototype's BDI/BAI mood-check detail was found and deliberately excluded. `CourseCatalog.cs` Week 6 route set; `Kurs.razor` start-panel updated for 5 available weeks. New `Week6ContentSliceTests.cs` + minimal updates to 6 existing test files (available-week-count assertions). 372/372 tests, build 0/0, 15/15 routes 200. **Status: `WEEK 6 — COMPLETE`, committed (hash `ac0d82e`)**. Next authorized candidate from the frozen order: **Week 12** — not started automatically.

### O.1 Status

`WEEK 6 — COMMITTED (ac0d82e)`. Optional Reading Visual Refinement closed separately, committed (`70d56cb` + `3bd527e`, Сесия 36).

---

## Systematic Curriculum Expansion — Phase C: Week 12 Implementation (Сесия 37, 2026-08-14)

Second position of the frozen build order (§J above) implemented. Full narrative in `10_SESSION_LOG.md` (Сесия 37). Summary: `Sedmica12.razor` built with zero new components/CSS (reuses `LearningSection`/`LearningObjectives`/`ProgressiveExplanation`/`DisclaimerCallout`/`SourceReferences`/`OptionalReadingSource`, `.category-compare`, corrected-pattern `.learning-grid--balanced`); source contract held to SRC-041 Гл. 14 exactly — the three negative-core-belief categories (безпомощност/необичаемост/безполезност, Beck 1999 + J.S. Beck 2005), explicitly reconciled against Week 3's already-published "core beliefs are not inherently negative" claim rather than contradicting it. `CourseCatalog.cs` Week 12 route set; `Kurs.razor` start-panel deliberately **not** updated — Week 12 resolves to `CourseWeekStatus.AcademicOverview` via `DeriveStatus()` (its `CurriculumSafetyLevel.AcademicContextOnly`), not `Available`, despite having a real route, so it does not join the five-week "Откъде да започнете" list; it appears only in the hub timeline with a working link and an "Академичен обзор" badge. New `Week12ContentSliceTests.cs` (21 facts) + minimal updates to 6 existing test files (route-null exception for the now-routed-but-not-Available Week 12). 396/396 tests, build 0/0, 9/9 routes 200. **Status: `WEEK 12 — COMPLETE`, uncommitted.** Next authorized candidate from the frozen order: **Week 7** — not started automatically. Deployment remains out of scope by design (local-first until a dedicated future phase).

### O.2 Status

`WEEK 12 — COMPLETE, UNCOMMITTED — AWAITING OWNER APPROVAL FOR SINGLE COMMIT`

---

## Deep Learning pivot — Week 6 v2 full rebuild (Сесия 38–39, 2026-08-14)

**Direction change (Сесия 38):** owner paused the frozen curriculum build order after reviewing
Weeks 6/8/10/12 and judged them too short/shallow for the platform's real goal — deep, systematic,
source-faithful CBT learning, not brief web pages. `Week 12` implementation is **not abandoned**,
just superseded in priority — it stays uncommitted, unchanged, ready to resume once the Deep
Learning model is validated. New standard: a "week" must be a real, deep learning module, built
from the **full text** of its source chapter, not a session-log one-line summary.

**Blueprint phase (Сесия 38):** `WEEK6_v2_DEEP_LEARNING_BLUEPRINT.md` written and revised to v1.1
(owner-approved, `READY FOR IMPLEMENTATION: YES`) — full narrative in `10_SESSION_LOG.md` Сесия 38.
Key finding: SRC-041's full text existed only as a one-time chat paste (Сесия 3, 2026-07-29),
never persisted to a file — the "100% FULLY REVIEWED" registry status was misleading for reuse
purposes. Owner supplied the local PDF path; full Глава 5 text (printed стр. 59–79, ~41,600
chars) extracted via `pypdf`, cached at `00_PROJECT_OS/_source_corpus/` (gitignored, never
committed — copyrighted book text). 47 knowledge units identified, Chapter Coverage Matrix built,
100% accounted for.

**Implementation (Сесия 39):** `Sedmica6.razor` fully rebuilt per the blueprint — 14 sections
(6.0–6.13), all 47 units represented, first-session precision applied throughout (Глава 5 = the
FIRST session specifically; v1's own "стандартна сесия" generalization was a real, now-fixed
source-fidelity defect). Three new, individually-justified reusable components: `WhatIfBox`
(reproduces SRC-041's own recurring "Какво ако...?" device), `SourceArtifact` (Figures 5.1/5.2,
own design, not pixel-copy), `ScenarioSimulator` (Interactive WebAssembly, data-driven,
Level A Recognition / Level B Application / Level C branching multi-step Reasoning — component
itself generic/reusable, Week-6 content supplied via parameters). New `.decision-branch` CSS
pattern (real root-to-leaves branching diagram, replacing a flat `.category-compare` grid that
was flagged as not a genuine branch). Case Lab: Мартин (Basic), Ирина (Intermediate — approved
pilot longitudinal case), Радо (Challenging — also the Level C simulator's scenario basis). U08/U22
(risk-screening content) included per the owner-approved `OBSERVATIONAL SAFETY BOUNDARY` contract
— third-person only, no self-screening, no scoring, no input fields. 13 local knowledge checks +
20-question final assessment (Q19 rewritten from platform-safety-policy to source-grounded CBT
content per owner instruction), all with explanatory, source-cited feedback.

**Tests:** new `Week6ContentSliceTests.cs` (full rewrite, 33 facts) + new `Week6NewComponentTests.cs`
(17 facts) + `OptionalReadingSourceTests.cs` chapter-text update (1 line, first-session precision).
**430/430 passing** (396 baseline + 34 net new). Build 0/0. 9/9 routes 200 on fresh instance.

**Status:** `WEEK 6 v2 — IMPLEMENTED, AWAITING OWNER LEARNING REVIEW` (not `COMPLETE` — per the new
Deep Learning Week DoD v2, `06_QA_STRATEGY.md`, owner learning review is the final gate, not
automatic even with green tests/build/coverage). Uncommitted, isolated from the still-pending,
still-uncommitted Week 12 diff. **Не започвай Week 12, Week 7, или redesign на друга седмица** —
awaiting owner review of this module first.

### Cognitive Learning Architecture — Reference Implementation, Phases 1-3 (Сесия 42, 2026-08-22)

- **Повод:** owner-authorized implementation of `00_PROJECT_OS/_blueprints/COGNITIVE_LEARNING_ARCHITECTURE_v1.md`
  v1.1 (`READY FOR IMPLEMENTATION PROMPT: YES`) — Phases 1-3 only (semantic models/contracts,
  `ConceptGraph` rendering engine, Week 6 reference implementation), followed by an owner review
  gate before Phase 5 (global Course Map / CBT Knowledge Map on `/kurs/karta`) begins. [Corrected
  Сесия 46 — this line originally said "Phase 4" here, inconsistent with the Phase numbering used
  in this same entry's own Status line below; Phase 4 is the owner review gate itself, Phase 5 is
  the global maps.]
- **Phase 1 — Semantic models:** `Curriculum/ConceptGraphModels.cs` (`MindMapNode`/`MindMapModel`,
  `ConceptNode`/`ConceptRelation`/`ConceptMapModel`, `ConceptState` enum + `ConceptStateResolver`
  mirroring `CurriculumLabels.DeriveStatus()`), `Curriculum/CaseConceptualizationModels.cs`
  (`CaseCharacter`/`CaseObservation`/`CaseConceptualizationModel` + `CaseCatalog` holding Ирина's
  single Week 6 observation — no invented future history).
- **Phase 2 — Rendering engine:** `Curriculum/ConceptGraphAdapters.cs` (`GraphRenderNode`/
  `GraphRenderEdge`/`GraphRenderModel`, `MindMapAdapter`/`ConceptMapAdapter`/
  `CaseConceptualizationAdapter` — domain models never reach the renderer directly) +
  `Components/Shared/ConceptGraph.razor` (Static SSR, no `@rendermode`; MindMap/ConceptMap/CaseMap
  modes; accessible `<details>` fallback generated from the same `Model.Nodes`/`Model.Edges` the
  visual canvas reads, per architecture §13/§19). New `.concept-graph` CSS block in `app.css`,
  reusing the `.decision-branch` SVG-connector technique and the `.concept-map__flow` vertical-chain
  technique — no new graph library.
- **Phase 3 — Week 6 reference implementation:** Weekly Mind Map (Preview in 6.0, Review in 6.12 —
  both bind the *same* `_week6MindMapRender` instance, gated by an attempt-before-reveal `<details>`
  for Review); Concept Map in 6.5 upgraded from a flat 3-node `.concept-map__flow` chain to a
  ConceptGraph instance with real cross-links (Телесна реакция/Поведение → Седмица 8's already-built
  full model; Междинни/основни вярвания → Седмица 3) — nothing invented, only pointers to curriculum
  that already exists; Case Conceptualization Map for Ирина in 6.8 (only `Situation`/`Behavior`/
  `InterventionLink` populated — the only fields Week 6's own text actually establishes for her);
  two retrieval-practice additions (explain-before-reveal prompt on the 11-term flashcard grid;
  attempt-before-reveal reconstruct-the-order widget in 6.12, SSR-only via nested `<details>`, not
  an extracted WASM widget — Static SSR stays the default per owner decision, architecture §21/§28).
- **Tests:** new `ConceptGraphModelTests.cs` (14 facts — resolver logic, adapter validity, dangling-
  parent guard, Case Map progressive-disclosure guarantee, Ирина no-invented-history guard) + new
  `Week6CognitiveMapTests.cs` (14 facts — Preview/Review share one model, concept-map cross-links,
  Ирина case map, retrieval-opportunity presence, ConceptGraph accessibility contract) +
  `Week6ContentSliceTests.cs` updated (1 assertion: `.concept-map__flow` → `<ConceptGraph`, since
  the old flat chain was merged into the new component, not left duplicated). **458/458 passing**
  (430 baseline + 28 net new). Build 0/0, 0 warnings. Fresh-instance smoke: `/`, `/kpt`, `/kurs`,
  `/kurs/sedmica-1`, `/kurs/sedmica-3`, `/kurs/sedmica-8`, `/kurs/sedmica-10`, `/kurs/sedmica-6` all
  `200`. Structural HTML inspection (no browser/screenshot tool in this environment) confirmed all
  4 ConceptGraph instances render with correct, unique ids, correct node/edge content, and no bare
  fragment anchors — **structural visual QA only**, owner must confirm the real visual/learning
  quality.
- **Status:** `WEEK 6 COGNITIVE REFERENCE IMPLEMENTATION — AWAITING OWNER REVIEW`. Project-wide
  architecture status: `REFERENCE VALIDATION IN PROGRESS` — Phase 4 (owner visual + learning
  review) is the gate before Phase 5 (global Course Map / CBT Knowledge Map) begins. **Explicitly
  not built in this step:** `/kurs/karta`, global Course Map, global CBT Knowledge Map, retrofit of
  Weeks 1/3/8/10/12, Week 7. Deep Learning DoD v2 → v3 applied in `06_QA_STRATEGY.md` (mechanical
  "minimum 3 visuals" replaced by Cognitive Representation Coverage + new Retrieval Practice
  Coverage gates). Uncommitted at write time — pending the single implementation commit described
  in `10_SESSION_LOG.md` (Сесия 42), isolated from the still-pending, still-uncommitted Week 12 diff.

### Phase 4 Gate — Owner Visual + Learning Review — PASSED (Сесии 43–46, 2026-08-22)

- Three visual-correction iterations followed the Phase 1-3 reference implementation above, each a
  separate, narrowly-scoped session, each committed independently: Сесия 43 (`30f39e4`, outline-tree
  MindMap rendering replacing the card grid), Сесия 44 (`5b7c0ec`, left-to-right desktop spatial
  reflow via `@container`/`@supports`, same markup/data, no new component), Сесия 45 (`4ea9619`,
  connector/navigation-affordance polish). No semantic model, adapter, ConceptMap, CaseMap, or Week
  6 source content changed across any of the three — visual/CSS layer only, confirmed by regression
  QA at each step.
- **Сесия 46 — owner reviewed `/kurs/sedmica-6` in a real browser and approved.** Locked as the
  project-wide visual/architecture reference standard (full LOCKED list in `02_CURRENT_STATUS.md`).
  Last approved commit: `4ea9619`.
- **Phase 4 gate: `PASSED`.** Project-wide Cognitive Learning Architecture reference validation:
  `PASSED`. Deep Learning DoD v3 (`06_QA_STRATEGY.md`) stands confirmed as applied.
- **Open, non-blocking content QA item:** GAP-013 (`15_GAPS_AND_CONFLICTS.md`) — Week 6's own text
  claims "six goals" but enumerates only 5 clauses; needs a direct re-check against SRC-041 Chapter
  5 in a future source-fidelity pass. Not invented, not fixed by guesswork, does not block this gate.
- **Next authorized phase: Phase 5 — Course Map + CBT Knowledge Map on `/kurs/karta`**, two distinct
  modes (Course Map: modules/weeks/prerequisites/status/routes; CBT Knowledge Map: concepts/labeled
  relationships/introduced-revisited metadata/cross-week links/anchors) — **not started**, requires
  a separate, new owner-authorized implementation task. Phase 6 (retrofit of routed weeks) and Phase
  7 (resume Week 7) remain frozen behind it, per the original Phase 1-7 sequence.

### Phase 5 — Global Course + Knowledge Maps — IMPLEMENTED (Сесия 47, 2026-08-22)

- **New route `/kurs/karta`**, two modes selected via `?mode=course` (default) / `?mode=knowledge`,
  read with `[SupplyParameterFromQuery]` — a full server-rendered navigation between two real,
  deep-linkable URLs, zero JS/WASM for the switch itself. Static SSR throughout.
- **Course Map** (`Curriculum/CourseMapBuilder.cs`): derives its `MindMapModel` entirely from
  `CourseCatalog.Modules`/`CourseCatalog.Weeks` at build time — no second, hand-maintained dataset.
  Root ("15-седмичен курс") → 4 modules → 15 weeks; `ConceptState.Introduced` = has a real `Route`
  (reachable), `ConceptState.Upcoming` = `Route == null` — curriculum reachability, never learner
  progress. Cross-module prerequisite links are deliberately not drawn (Mind Map mode is a strict
  single-parent tree; that concern belongs to the Knowledge Map instead).
- **CBT Knowledge Map** (`Curriculum/KnowledgeMapCatalog.cs`): a small, honest `ConceptMapModel` — 10
  concepts / 12 relations, every one verified against the actual, already-routed page content of
  Weeks 3, 6, 8, 10 (real anchors: `#situacia-znachenie`, `#tri-niva`, `#karta-na-temata`,
  `#poniatiya`, `#izsledvane`; Week 12 contributes only as a Revisited week for "Основно вярване",
  never as an IntroducedWeek). Week 1 deliberately contributes no concept (purely historical/
  narrative content). A real source-fidelity nuance surfaced during grounding: Week 3's own
  "Ситуация→значение" chain uses "Автоматично значение" and a combined "Емоционална и телесна
  реакция" — Emotion and Body reaction only become separately-named concepts in Week 8's diagram, so
  their `IntroducedWeek` is correctly 8, not 3 (Week 6's own, separately-locked concept map is
  unaffected — not retroactively "corrected").
- **Zero changes to the locked Week 6 reference or to `ConceptGraph.razor`/`MindMapBranch.razor`** —
  both maps reuse the existing renderer exactly as Week 6 left it. `Kurs.razor` gained one visible
  link to `/kurs/karta` (§18 requirement); a Week 6 → `/kurs/karta` back-link was **not** added (§19
  — would require touching the locked file), logged here as a future retrofit item instead.
- **Tests:** new `GlobalMapsTests.cs` (19 facts — Course Map derivation/state/hierarchy validity,
  Knowledge Map ID/endpoint/anchor/week-reference validity, route/page structural checks). **495/495
  passing** (476 baseline + 19 new). Build 0/0. Fresh-instance smoke: 10/10 routes `200`, including
  both `/kurs/karta` modes. Week 6 regression confirmed intact (identical root label, identical node
  count). **Structural QA only** — no browser/screenshot tooling in this environment.
- **Status:** `GLOBAL COURSE + KNOWLEDGE MAPS — IMPLEMENTED, AWAITING OWNER VISUAL + LEARNING
  REVIEW`. Not committed at write time. Explicitly not done in this step: retrofit of Weeks
  1/3/8/10/12 pages, Week 7, any change to Week 6, GAP-013 resolution, any new graph library.
