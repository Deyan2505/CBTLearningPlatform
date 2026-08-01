# 04 — Changelog

Формат: най-новото най-отгоре. Всеки запис отбелязва датата и типа промяна (добавено / променено / поправено / премахнато / архитектура / база данни / документация).

---

## 2026-08-01 (продължение 5) — STEP-3.4: Модул 1 overview и първи реален урок

- **Съдържание (добавено):** `/programa/modul-1` — Модул 1 overview ("Какво представлява КПТ"). `/programa/modul-1/kakvo-e-kpt` — единственият одобрен урок (Модул 1 е едноурочен по IA, не измислена подструктура). Нов пример (приятел споделя тревога), илюстриращ границата образование/терапия. Обща, добре подкрепена evidence-base claim (REQ-CLIN-009) — специфичната 2006 статистика умишлено изключена.
- **Код (поправено):** `Programa.razor`/`Home.razor` — Модул 1 картите вече сочат към реалното съдържание (вместо disabled state от STEP-2.1/2.2). Урок 1 линква напред към реалния Модул 2 (Модул 1 няма собствен урок 2).
- **Тестове (добавено):** 8 нови теста в `ContentSliceTests.cs` (Модул 1/Урок 1 съществуват, overview линква към урока, урокът линква към реални Модул 1/Модул 2 routes, Programa/Home линкват към реалния Модул 1 overview, disclaimer/distortion-categorization разширени). Общо 38/38 passing.
- **Проверено:** build 0/0; реален HTTP smoke test на всички routes (200), несъществуващ Модул 1 урок 2 → friendly 404, disclaimer навсякъде, 0 disabled карти останали на Home/Programa (и двата модула вече напълно реални).
- **Резултат:** `PHASE 3 / STEP-3.4 COMPLETE`.

## 2026-08-01 (продължение 4) — STEP-3.3: трети урок, ADR-009 реконсилиация, Content Pipeline Decision

- **Архитектура (ново решение, ADR-009):** флагманският Модул 2 официално поема базовото ниво на темите от бъдещите Модул 3 ("Автоматични мисли") и Модул 5 ("Емоции и телесни реакции"). Тези бъдещи модули се стесняват — Модул 3 → задълбочено записване на мисли (мост към thought record); Модул 5 → по-задълбочена емоционална регулация. `18_INFORMATION_ARCHITECTURE.md` актуализиран с анотации на Модул 2/3/5.
- **Съдържание (добавено):** `/programa/modul-2/emocii-i-telesni-reaktsii` — Урок 3 "Емоции и телесни реакции" (SRC-041 Гл. 10). Нов пример (закъснение в трафик, различен от предходните два); липса на отделна "Сравнение" секция (не се налагаше за тази тема); "Забележете" callout; 4 рефлективни въпроса; 5 извода — с този урок трите урока покриват целия модел.
- **Код (поправено):** `Modul2.razor` — трета `ModuleCard`. `Modul2Lesson2.razor` — "Следваща стъпка" вече линква реално към Урок 3.
- **Тестове (добавено):** 5 нови теста в `ContentSliceTests.cs`. Общо 30/30 passing.
- **Content Pipeline Decision (анализ, без имплементация):** сравнени 3 реални урока по 13 критерия (route metadata, structure, examples, callouts, questions, summary, disclaimer, navigation, sources, review status). **Решение: `KEEP RAZOR FOR MVP`** — content е тясно свързан с shared компоненти и structural markup (карти/callouts), REQ-CONT-002 таванира флагманския модул на 2–4 урока (вече достигнат), pipeline за максимум 1 бъдещ урок в тази граница не носи пропорционална стойност. Пълен анализ в `24_IMPLEMENTATION_ROADMAP.md` (STEP-3.3).
- **Проверено:** build 0/0; реален HTTP smoke test на всички routes (200), несъществуващ Урок 4 → friendly 404, disclaimer навсякъде.
- **Резултат:** `PHASE 3 / STEP-3.3 COMPLETE`.

## 2026-08-01 (продължение 3) — STEP-3.2: втори реален урок от Модул 2

- **Съдържание (добавено):** `/programa/modul-2/avtomatichni-misli` — Урок 2 "Автоматични мисли" (SRC-041 Гл. 9). Нов, различен от Урок 1, ежедневен пример (готвене по нова рецепта); сравнение мисъл-срещу-чувство; "Забележете" callout; 3 рефлективни въпроса; обобщение.
- **Съдържание (реконсилиация):** темата произхожда от одобрения content map запис "Модул 3 — Автоматични мисли", но реализирана като Урок 2 на флагманския Модул 2 (REQ-CONT-002 "2–4 lessons in flagship module"), не отделна Модул 3 страница.
- **Код (поправено):** `Modul2.razor` — добавена втора `ModuleCard` за Урок 2. `Modul2Lesson1.razor` — "Следваща стъпка" вече линква реално към Урок 2 (вместо честното "предстои" от STEP-3.1).
- **Тестове (добавено):** 4 нови теста в `ContentSliceTests.cs` (Lesson 2 съществува; overview линква към двата урока; Урок 1 вече без dead-end "предстои"; Урок 2 back-links сочат към реални routes). Общо 25/25 passing.
- **Проверено:** build 0/0; реален HTTP smoke test (всички routes 200, несъществуващ Урок 3 → friendly 404, disclaimer на всички съдържателни страници, source citation потвърдена чрез HTML entity decode).
- **Резултат:** `PHASE 3 / STEP-3.2 COMPLETE`.

## 2026-08-01 (продължение 2) — STEP-3.1: първи реален обучителен content slice (Фаза 3 стартирана)

- **Съдържание (добавено):** `/kpt` — публична страница "Какво е когнитивно-поведенческа терапия?" (модел, ход на обучението, граници на платформата).
- **Съдържание (добавено):** `/programa/modul-2` — Модул 2 overview (учебни цели, списък уроци, статус).
- **Съдържание (добавено):** `/programa/modul-2/situacia-misal-emocia-povedenie` — първи реален урок (модел, оригинален неутрален пример с 2 алтернативни интерпретации, "Забележете" callout, 3 рефлективни въпроса, обобщение). Базирано на REQ-CLIN-003 (SRC-041, Гл. 3) и REQ-CONT-002/005.
- **Код (добавено):** `LearningObjectives.razor` и `SourceReferences.razor` — reusable компоненти (реално използвани 2×: overview + урок).
- **Код (поправено):** Модул 2 картите на `Home.razor`/`Programa.razor` вече сочат към реалното съдържание (`DestinationUrl`), вместо disabled state.
- **Поправено (реален бъг, открит по време на изпълнение):** `/kpt` и урокът първоначално нямаха `DisclaimerCallout` — открито чрез HTTP smoke test, коригирано; добавен регресионен тест (`PsychologicalContentPage_IncludesDisclaimerCallout`).
- **Clinical safety:** съдържанието маркирано `REQUIRES PROFESSIONAL REVIEW` (Razor коментари в трите нови файла + `13_REQUIREMENTS_TRACEABILITY.md`) — не е публикувано за реални потребители без клиничен рецензент (RISK-010). REQ-CLIN-008 (10 принципа) и REQ-CLIN-009 (evidence-base статистика от 2006) съзнателно изключени от тази стъпка.
- **Тестове (добавено):** `ContentSliceTests.cs` (12 теста) + `TestPaths.cs` (споделен helper, извлечен от `DesignSystemTests.cs` при втора реална употреба). Общо 21/21 passing.
- **Проверено:** build 0/0; реален HTTP smoke test на всички нови routes (200), несъществуващ lesson route (404 през friendly NotFound), disclaimer на всички съдържателни страници, липса на неподкрепената категоризация на изкривяванията.
- **Резултат:** `PHASE 3 / STEP-3.1 COMPLETE`.

## 2026-08-01 (продължение) — STEP-2.2: реални публични страници (Home + Programa)

- **Код (поправено):** `Home.razor` пренаписана — hero (заглавие/обяснение/primary+secondary CTA), "Как работи обучението" (3 честни стъпки), "Какво ще научите" (2 `ModuleCard`), "Образователна граница" (`DisclaimerCallout` + пълен "какво НЕ е" списък от `00_PROJECT_CHARTER.md`).
- **Код (добавено):** нова `/programa` страница — модулен каталог с 2 `ModuleCard` + `DisclaimerCallout`.
- **Код (добавено):** `Components/Shared/ModuleCard.razor` — reusable компонент (Title/Description/StatusLabel/DestinationUrl/CtaLabel); при липсващ `DestinationUrl` рендира честен disabled-state вместо dead link.
- **Код (поправено):** `MainLayout.razor` header nav — `<a>` заменени с `NavLink` за автоматичен active state (`class="active"` + `aria-current="page"`, потвърдено в реално сервирания HTML).
- **CSS (добавено):** `.site-nav a.active`, `.module-list`, `.module-card__status`, `.is-disabled` в `app.css`.
- **Google Fonts CDN:** запазен временно (Variant B) — не разширен, fallback stack потвърден в `--font-family-base`, регистриран като отворено privacy/performance решение, не окончателно production.
- **Тестове (добавено):** `PublicPagesTests.cs` — 3 нови теста (Programa съществува, ModuleCard съществува, ModuleCard public API стабилен). Общо 9/9 passing.
- **Проверено:** build 0/0; реален HTTP smoke test на `/` и `/programa` (200), active nav state, disabled module CTA (без href), липса на dead links, липса на placeholder текст, точен disclaimer текст на двете страници.
- **Резултат:** `PHASE 2 / STEP-2.2 COMPLETE`. Phase 2 критерий "поне 2 реални страници" изпълнен.

## 2026-08-01 — STEP-2.1: Design system foundation (Фаза 2 стартирана)

- **Архитектура:** STEP-1.6 (linting) официално `DEFERRED — REQUIRED BEFORE FIRST MAJOR FEATURE MERGE OR REMOTE PR` — собственическо решение, регистрирано в `24_IMPLEMENTATION_ROADMAP.md`.
- **Код (добавено):** `app.css` пренаписан с пълна design tokens система (color roles, typography, spacing, shape, layout) + базови компоненти (бутони, връзки, карти, callout, navigation, form field foundation).
- **Код (добавено):** `Components/Shared/DisclaimerCallout.razor` — reusable компонент с точния одобрен disclaimer текст от `23_CLINICAL_SAFETY_BOUNDARIES.md`.
- **Код (поправено):** `MainLayout.razor` — реална header/nav (по одобрената IA)/footer/skip-link структура; `MainLayout.razor.css` restyled с tokens вместо hardcoded цветове; `App.razor` `lang="en"` → `lang="bg"`.
- **Код (поправено):** `Home.razor` и `NotFound.razor` — честно, минимално реално съдържание вместо template placeholder ("Hello, world!" премахнато).
- **Skill:** `ui-ux-pro-max` (SKILL-046) използван за starting point (style/color/typography/ux заявки), адаптиран ръчно — не механично прието.
- **Тестове (добавено):** `DesignSystemTests.cs` — 2 нови теста. Общо 6/6 passing.
- **Проверено:** build 0/0; реален HTTP smoke test (home 200, app.css 200, не-построена nav страница коректно 404 през нашата friendly NotFound страница); структурни проверки в реално сервирания HTML (nav/header/footer/disclaimer/skip-link/lang).
- **Резултат:** `PHASE 2 / STEP-2.1 COMPLETE`.

## 2026-07-30 (продължение 7) — STEP-1.5: обработка на грешки

- **Код (добавено):** `<ErrorBoundary>` около `<RouteView>` в `Routes.razor` — минимален български fallback за static SSR слоя, без активиране на global interactive rendering.
- **Код (поправено):** `Error.razor` пренаписана на спокоен, кратък български текст без технически подробности; премахнат dev-ориентираният обяснителен блок от темплейта. `RequestId` (безопасен correlation id) запазен.
- **Код (поправено):** `#blazor-error-ui` банер в `MainLayout.razor` преведен на български — `id`/класове непроменени (ползвани от `blazor.web.js`).
- **Код (без промяна):** server-side `UseExceptionHandler`/`UseHsts`/`UseStatusCodePagesWithReExecute` вече присъстваха в темплейта — не дублирани, не заменени с custom `IExceptionHandler`/`AddProblemDetails` (не е API проект).
- **Проверено (реално, не само предположено):** временен diagnostic endpoint (добавен и премахнат в рамките на сесията) хвърлен в истинска Production среда (compiled DLL, не `dotnet run`, за да се заобиколи `launchSettings.json` override на `ASPNETCORE_ENVIRONMENT`) → HTTP 500, приятелско съобщение, 0 изтекли exception детайли.
- **Тестове (добавено):** `ErrorHandlingTests.cs` — 2 reflection-базирани теста. Общо 4/4 passing.
- **Резултат:** `PHASE 1 / STEP-1.5 COMPLETE — REMOTE CI RUN PENDING`.

## 2026-07-30 (продължение 6) — STEP-1.4: xUnit test project + CI test step

- **Код (добавено):** `CbtLearningPlatform.Tests/` — xUnit test project (`net10.0`, официален `dotnet new xunit` template, немодифицирани package versions), добавен към solution, project reference към host `CbtLearningPlatform`.
- **Код (добавено):** `RuntimeBaselineTests.cs` — 2 инфраструктурни теста (target framework на компилирания test assembly чрез `TargetFrameworkAttribute`, host assembly loadable чрез `Assembly.Load`). Генерираният `UnitTest1.cs` премахнат.
- **Поправено (root cause по време на изпълнение):** първоначален подход с `AppContext.TargetFrameworkName` връщаше грешен резултат (`v8.0` вместо `v10.0`), защото отразява entry assembly на VSTest testhost процеса, не самия test проект — сменено с директно четене на `TargetFrameworkAttribute` от компилирания test assembly.
- **CI (добавено):** `.github/workflows/ci.yml` разширен с `Test` стъпка (`dotnet test ... --no-build --no-restore`) след `Build`, без промяна на triggers/permissions/runner/timeout.
- **Проверено:** `dotnet build --configuration Release` → 0/0; `dotnet test` → 2/2 passed, 0 failed, 0 skipped.
- **Резултат:** `PHASE 1 / STEP-1.4 COMPLETE — REMOTE CI RUN PENDING`. Реален GitHub Actions run остава невъзможен — няма remote. STEP-1.5 не е изпълнена.

## 2026-07-30 (продължение 5) — STEP-1.3: CI workflow (локално конфигуриран)

- **CI (добавено):** `.github/workflows/ci.yml` — единствен workflow: restore + Release build на `CbtLearningPlatform.sln`, `permissions: contents: read`, SDK версия от `global-json-file` (без дублиране), `actions/checkout@v7`, `actions/setup-dotnet@v6`, triggers `push`/`pull_request` към `main` + `workflow_dispatch`. Без caching (няма `packages.lock.json`), без фиктивен `dotnet test` (няма test project).
- **Проверено:** структурен YAML преглед (без tabs, без duplicate keys, коректни пътища спрямо реалната файлова структура), security review (без secrets, без write permissions, без опасни triggers/actions), локален `dotnet restore` + `dotnet build --configuration Release` → 0/0.
- **Ограничение:** реален GitHub Actions run **не е възможен** — repository няма remote; не е създаден такъв в тази стъпка.
- **Резултат:** `CI CONFIGURATION COMPLETE — REMOTE RUN PENDING`. STEP-1.4 не е изпълнена.

## 2026-07-30 (продължение 4) — STEP-1.2 завършена: Git identity + baseline commit

- **Version control (добавено):** Git identity зададена **локално** (`git config --local`, само за това repository — global конфигурация непроменена) след изрично собственическо потвърждение на реален email.
- **Version control (добавено):** baseline commit създаден, съдържащ цялата одобрена основа — `.gitignore`, всички Project OS документи (вкл. Context Control актуализациите), Blazor solution, `code_artifact.html`.
- **Process (добавено):** постоянно Context Control правило в `01_MASTER_PLAN.md` + `ACTIVE CONTEXT FOR CURRENT STEP` секция в `02_CURRENT_STATUS.md` — включени в baseline commit.
- **Резултат:** `PHASE 1 / STEP-1.2 COMPLETE`. STEP-1.3 не е изпълнена.

## 2026-07-30 (продължение 3) — STEP-1.2 (PARTIAL)

- **Version control (добавено):** Git repository инициализиран в project root (не в `CbtLearningPlatform/`) — потвърдено чрез `git rev-parse --show-toplevel`. Branch `main` (преименуван от `master`, нов repo без история).
- **Version control (добавено):** официален `.NET .gitignore` темплейт (`dotnet new gitignore`) — покрива `bin/`/`obj/`/`.vs/`/publish/secrets patterns; `.vscode/` игнорирана селективно (не изцяло), критичните файлове (Project OS, `code_artifact.html`, `.sln`, `.csproj`, `global.json`) остават trackable — валидирано с `git check-ignore`.
- **Version control:** всички 48 очаквани файла staged (`git add .`); 0 secrets/bin/obj/installer файлове открити при одита.
- **Блокирано:** baseline commit **не е създаден** — липсва Git identity (`user.name`/`user.email`, нито local, нито global). Изисква собственикова намеса преди commit.
- **Проверено:** `dotnet restore` + `dotnet build` след Git init — 0 Warning(s)/0 Error(s), без регресия спрямо STEP-1.1.
- **Без промяна по код:** `code_artifact.html` остава недокоснат. STEP-1.3 не е изпълнена.

## 2026-07-30 (продължение 2) — .NET 10 SDK инсталация + STEP-1.1

- **Environment (добавено):** .NET 10 SDK (`10.0.302`, GA) инсталиран автономно чрез официалния WinGet пакет `Microsoft.DotNet.SDK.10`. Съществуващите runtime версии (6.0.35, 8.0.17) запазени непроменени.
- **Код (добавено):** създаден Blazor Web App solution `CbtLearningPlatform/` — 2 проекта (`CbtLearningPlatform` хост + `CbtLearningPlatform.Client` за WebAssembly интерактивност), `.sln`, `global.json` (SDK pin). Target framework `net10.0`, `--empty` темплейт, без authentication.
- **Проверено:** build чист (0/0), `dotnet run` реално обслужва началната страница (HTTP 200).
- **Без промяна по код:** `code_artifact.html` остава недокоснат. Git repository все още не е инициализиран (STEP-1.2, не изпълнена).

## 2026-07-30 (продължение) — Claude Code Skills Audit + Environment Correction

- **Поправено:** environment статусът от предходния запис е коригиран — единственият реален `REQUIRED` блокер е липсващият .NET 10 SDK. VS Code, Claude Code и Git са напълно функционални; пълен Visual Studio IDE е `RECOMMENDED`, не `REQUIRED`.
- **Документация (добавено):** `25_CLAUDE_CODE_SKILLS_REGISTRY.md` (пълен inventory + selection matrix), `26_SKILL_USAGE_LOG.md` (seeded, празен до първо реално изпълнение на стъпка).
- **Процес (добавено):** постоянно правило за skills discovery преди всяка съществена задача, регистрирано в `01_MASTER_PLAN.md`.
- **Резултат:** потвърдено — няма specialized skill за Blazor/.NET/UI-UX/accessibility в тази среда; общите skills (ponytail, simplify, security-review) остават приложими.

## 2026-07-30 — Owner Approval Gate + Phase 1 Entry Check

- **Архитектура (поправено):** технологичният стек Blazor Web App е потвърден от собственика, но версията е коригирана от .NET 8/9 (както беше решено на 2026-07-29) на **.NET 10 LTS** (ADR-007). Всички текущи (не исторически) документи, посочващи .NET 8/9, са актуализирани.
- **Архитектура (добавено):** изрична Blazor rendering стратегия — Static SSR по подразбиране, Interactive WebAssembly (не Server) само за компонентите с лично съдържание на потребителя (ADR-007).
- **Документация:** одобрени от собственика документите от Phase 0E (`16`, `18`, `19`, `20`, `21`) — статус актуализиран от `PROPOSED`/без статус на `OWNER APPROVED` (с уточнения за всеки).
- **Решение:** категоризацията на изкривяванията "Оценка/Прогнозиране/Филтриране/Правила" окончателно НЕ се публикува в MVP в никаква форма (ADR-008).
- **Блокиращо (ново):** реална проверка на development средата установи липса на .NET SDK и Visual Studio инсталация на машината. Фаза 1 технически не е стартирана — статус `BLOCKED — DEVELOPMENT ENVIRONMENT`. Виж `02_CURRENT_STATUS.md`.
- **Без промяна по код:** `code_artifact.html` остава недокоснат; никакъв технически файл (solution/проект) не е създаден.

## 2026-07-29 — Фаза 0: инициализация на проекта

- **Документация:** създадена `00_PROJECT_OS/` с пълния комплект начални документи (charter, master plan, status, decision log, changelog, risk register, QA strategy, content governance, privacy & security, backlog, session log).
- **Архитектура:** избран технологичен стек — Blazor Web App (.NET 8/9). Виж ADR-001.
- **Архитектура:** решено MVP да е без потребителски акаунти (ADR-002) и без база данни за съдържание (ADR-003).
- **Без промяна по код:** `code_artifact.html` (предходен прототип) остава недокоснат в основната директория; не е част от новата архитектура.
