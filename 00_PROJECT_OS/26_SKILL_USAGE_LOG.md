# 26 — Skill Usage Log

Хронологичен, append-only. Запис само след реално извършена техническа/дизайн стъпка с реална връзка към използваните skills — не формално отбелязване.

---

## 2026-07-30 — STEP-1.1: Инициализация на Blazor Web App solution

- **Roadmap step:** STEP-1.1 (`24_IMPLEMENTATION_ROADMAP.md`).
- **Използвани skills:**
  - `ponytail:ponytail` (пасивно, YAGNI дисциплина) — приложено чрез избор на `--empty` темплейт (без демо Counter/Weather страници, които биха били веднага изтрити при Фаза 2 дизайн система) и чрез решение да не се добавя `dotnet new sln` отделно, тъй като темплейтът вече генерира `.sln` автоматично.
  - `run` — приложен за реалната проверка: стартиране на `dotnet run` във фонов процес, HTTP заявка към `http://localhost:5131`, потвърждение на HTTP 200 и реално HTML съдържание, последвано от чисто спиране на процеса.
  - `simplify` — **не е приложен**: скелето е генерирано изцяло от officiален Microsoft темплейт, няма създаден от мен код за опростяване на този етап.
  - `security-review` — **не е приложен**: STEP-1.1 не създава код, обработващ потребителски вход, автентикация или чувствителни данни (проверено — няма auth пакети, няма connection strings, няма API ключове в генерирания код).
- **Защо са избрани:** `run` е директно приложим за Definition of Done проверката ("проектът... стартира с една команда"); `ponytail` управлява темплейт избора към минимален отпечатък.
- **Конкретно приложено:** `dotnet new blazor -n CbtLearningPlatform -o CbtLearningPlatform -f net10.0 -int WebAssembly -au None -e`, последвано от `global.json` (SDK pin 10.0.302), `dotnet restore`, `dotnet build`, реален `dotnet run` + HTTP проверка + спиране на процеса.
- **Засегнати файлове:** 18 нови файла в `CbtLearningPlatform/` (виж пълния списък в handoff-а в чата).
- **Проверки:** build (0 грешки, 0 предупреждения), HTTP 200 от началната страница, target framework `net10.0` потвърден в двата `.csproj`, без secrets/DB/auth пакети, `code_artifact.html` непроменен, git repo не е инициализиран (STEP-1.2 недокоснат).
- **Отклонения от skill инструкциите:** няма.
- **Резултат:** STEP-1.1 завършена успешно, изцяло проверена.

---

## 2026-07-30 — STEP-1.2: Git repository и .gitignore (PARTIAL)

- **Roadmap step:** STEP-1.2 (`24_IMPLEMENTATION_ROADMAP.md`).
- **Използвани skills:**
  - `ponytail:ponytail` (пасивно, YAGNI дисциплина) — приложено чрез използване на официалния `dotnet new gitignore` темплейт вместо ръчно писан минимален `.gitignore`, и чрез запазване на `.vscode/*` селективно (не изцяло игнорирана), както темплейтът вече прави — без допълнителни ръчни правила, тъй като не бяха необходими.
  - `run` — приложен за реалните проверки: `git init`, `git rev-parse --show-toplevel`, `git check-ignore -v` (4 положителни + 4 отрицателни теста), `git add .` + `git diff --cached --stat`/`--check`, `dotnet restore`/`dotnet build` след Git init (регресионна проверка).
  - `security-review` — минимална проверка за secrets преди staging: сканиране за `.env`/`*.pfx`/`*.p12`/`*.pem`/`*.key`/`secrets.json`, съдържателна проверка на двата `appsettings*.json` за `apikey|secret|password|connectionstring|token` (празен резултат), проверка за файлове >10MB (няма). Не е пълен security review — не е бил необходим за обикновена Git инициализация.
  - `simplify` — **не е приложен**: няма нов/променен application код в тази стъпка, само repository/`.gitignore` файлове.
- **Защо са избрани:** `run` директно приложим за acceptance criteria проверката (`git status` чист, `bin/`/`obj/` игнорирани); `security-review` (минимален) — изрично изискан преди първи commit по roadmap инструкцията; `ponytail` управлява избора на официален темплейт пред ръчен.
- **Конкретно приложено:** `git init` в project root, `dotnet new gitignore`, `git branch -M main`, `git check-ignore -v` валидация (8 проверки), `git add .` (48 файла staged), `git diff --cached --stat`/`--check`, `dotnet restore` + `dotnet build` (0/0).
- **Засегнати файлове:** `.gitignore` (нов), `.git/` (нова директория, служебна). Staged, но не commit-нати: всички 48 съществуващи файла.
- **Проверки:** repository root = project root (не `CbtLearningPlatform/`); критичните файлове (Project OS, `code_artifact.html`, `.sln`, `global.json`) не са игнорирани; build чист след Git init; `code_artifact.html` непроменен; Git identity липсва (документирано, не заобиколено).
- **Отклонения от skill инструкциите:** commit частта на STEP-1.2 не е изпълнена — блокирана от липсваща Git identity, не от избор на skill. Не е измислена/зададена identity стойност.
- **Резултат:** STEP-1.2 `PARTIAL` — repository/`.gitignore`/staging завършени и проверени; baseline commit изчаква собственикова намеса.

**Допълнение (2026-07-30, продължение):** `run` използван за `git config --local` + verification + `git commit` + post-commit checks + build regression check, след като собственикът потвърди реален Git email при директна заявка. `security-review` (минимален) — повторен secret scan на unstaged Project OS промените преди финално staging. Резултат: STEP-1.2 `COMPLETE`.

---

## 2026-07-30 — STEP-1.3: Основен CI build (локално конфигуриран)

- **Roadmap step:** STEP-1.3 (`24_IMPLEMENTATION_ROADMAP.md`).
- **Използвани skills:**
  - `ponytail:ponytail` (YAGNI) — приложено чрез единствен минимален workflow (само restore+build), без caching (изисква lock файл, който не съществува), без фиктивен test step, без допълнителни workflows за security/deployment/accessibility.
  - `run` — реални Git/`.NET` проверки: initial state check, локален `dotnet restore`/`dotnet build --configuration Release`, `git status`/`git diff --check` след създаването на файла.
  - `security-review` (кратък) — преглед на workflow permissions/triggers/actions: потвърдено `contents: read` само, официални pinned actions, без `pull_request_target`, без remote scripts/secrets/artifact upload/deployment.
  - `simplify` — **не е приложен**: workflow-ът е минимален от самото начало, няма нужда от опростяване.
- **Защо са избрани:** `ponytail` директно governs решението да не се добавя caching/test/допълнителни workflows преждевременно; `security-review` — изрично изискан преди записване на CI permissions; `run` за Definition of Done локалната част (build проверка).
- **Конкретно приложено:** създаден `.github/workflows/ci.yml`, структурен YAML преглед (без tabs/duplicate keys), `dotnet restore` + `dotnet build --configuration Release --no-restore` (0/0), `git status --short` след build (само `.github/` untracked).
- **Засегнати файлове:** `.github/workflows/ci.yml` (нов).
- **Проверки:** пътища в YAML сверени с реалната файлова структура (`ls`); `actionlint` не е инсталиран — не е инсталиран автономно, документирано като ограничение.
- **Отклонения от skill инструкциите:** няма.
- **Резултат:** STEP-1.3 `CI CONFIGURATION COMPLETE — REMOTE RUN PENDING` — локалната конфигурация напълно проверена; реален GitHub run изисква remote, извън обхвата на тази стъпка.

---

## 2026-07-30 — STEP-1.4: xUnit test project + CI test step

- **Roadmap step:** STEP-1.4 (`24_IMPLEMENTATION_ROADMAP.md`).
- **Използвани skills:**
  - `ponytail:ponytail` (YAGNI) — приложено чрез: официален template без допълнителни библиотеки (без bUnit/Moq/FluentAssertions/AutoFixture), 2 честни теста вместо `Assert.True(true)`, project reference само към host (не и към `.Client`, без доказана нужда).
  - `run` — реално създаване на проекта, `dotnet sln add`, `dotnet add reference`, restore/build/test изпълнения (вкл. повторно след fix-а).
  - `security-review` (кратък) — потвърдено, че новите package references са само официалните template defaults (без непознати/трети зависимости), CI промяната не добавя permissions/secrets.
  - `simplify` — **не е приложен**: тестовият код е минимален от самото начало.
- **Защо са избрани:** `run` директно приложим за честна проверка на "минава ли `dotnet test`"; `ponytail` управлява решението да не се добавят test frameworks/абстракции без доказана нужда.
- **Конкретно приложено (вкл. root-cause fix):** `TestProject_TargetsDotNet10` първоначално използваше `AppContext.TargetFrameworkName`, който отразява entry assembly на VSTest testhost, не самия test проект — fail-на с `v8.0` вместо `v10.0`. Коригирано на четене на `TargetFrameworkAttribute` директно от компилирания test assembly (`typeof(RuntimeBaselineTests).Assembly`) — реалната, честна проверка. Ponytail принципът "root cause, not symptom" приложен буквално.
- **Засегнати файлове:** `CbtLearningPlatform.Tests/CbtLearningPlatform.Tests.csproj`, `CbtLearningPlatform.Tests/RuntimeBaselineTests.cs`, `.github/workflows/ci.yml`, `CbtLearningPlatform.sln`.
- **Проверки:** build (0/0), test (2/2 passed), package versions немодифицирани спрямо template, `bin`/`obj` на новия проект потвърдени игнорирани.
- **Отклонения от skill инструкциите:** няма.
- **Резултат:** STEP-1.4 `COMPLETE — REMOTE CI RUN PENDING`.

---

## 2026-07-30 — STEP-1.5: Обработка на грешки

- **Roadmap step:** STEP-1.5 (`24_IMPLEMENTATION_ROADMAP.md`).
- **Използвани skills:**
  - `ponytail:ponytail` (YAGNI, root-cause) — приложено чрез: запазване на съществуващата template server-side инфраструктура вместо дублиране/замяна с custom `IExceptionHandler`; минимален `<ErrorBoundary>` без глобален client exception service; при production верификацията — реален root-cause анализ защо `dotnet run` показваше Development (launchSettings.json override), вместо да се приеме резултат без обяснение.
  - `run` — реално създаване/build/test, и реално стартиране на compiled DLL в Production среда с временен diagnostic endpoint за честна проверка на acceptance criteria.
  - `security-review` (кратък) — потвърдено, че error UI не включва нови зависимости, не логва потребителско съдържание, не добавя external telemetry; временният diagnostic endpoint премахнат преди commit (потвърдено чрез `git diff`/byte-check на `Program.cs`).
  - `simplify` — **не е приложен**: кодът е минимален от самото начало.
- **Защо са избрани:** `ponytail` директно governs решението да не се строи exception framework; `run` — единствен начин да се докаже честно acceptance criteria (не просто да се твърди); `security-review` — изрично изискан преди commit заради временния diagnostic route.
- **UI UX Pro Max discovery:** проверени всички нива (project/parent/user `.claude/`, installed plugins, пълен marketplace catalog) — skill не съществува под никакво име. Не е "използван", защото не е бил открит — регистрирано честно, не твърдяно обратното.
- **Засегнати файлове:** `Routes.razor`, `Error.razor`, `MainLayout.razor`, `ErrorHandlingTests.cs` (нов). `Program.cs` — временна промяна, върната обратно преди commit.
- **Проверки:** build (0/0), test (4/4), реална Production HTTP проверка (500 без изтекли детайли), `git diff --check` чист, `Program.cs` потвърден непроменен спрямо преди сесията.
- **Отклонения от skill инструкциите:** няма.
- **Резултат:** STEP-1.5 `COMPLETE — REMOTE CI RUN PENDING`.

---

## 2026-08-01 — STEP-2.1: Design system foundation (Фаза 2, първо реално използване на `ui-ux-pro-max`)

- **Roadmap step:** STEP-2.1 (`24_IMPLEMENTATION_ROADMAP.md`, ново добавен).
- **Използвани skills:**
  - `ui-ux-pro-max` (SKILL-046) — **основен skill за тази стъпка**, задължителен по изрична инструкция. Реални CLI извиквания:
    1. `search.py "educational mental health calm trustworthy accessible" --design-system -p "CBT Learning Platform" -f markdown` — пълна design system препоръка (pattern/style/colors/typography).
    2. `search.py "education trust calm warm not clinical" --domain color -n 4` — алтернативни палитри след отхвърляне на предложената cyan/healthcare.
    3. `search.py "education long-form reading clear professional" --domain typography -n 4` — алтернативна типография след отхвърляне на wellness-mood Lora/Raleway.
    4. `search.py "accessibility forms focus keyboard" --domain ux -n 6` — потвърждение на accessibility правила.
  - **Приложени правила от skill-а:** focus-visible states (3-4px ring), keyboard navigation/tab order, skip link, 44×44px touch targets, `prefers-reduced-motion`, semantic input types (foundation, без реална форма още), style "Accessible & Ethical" (WCAG AAA насоченост), font "Atkinson Hyperlegible" (accessibility-first pairing).
  - **Отхвърлени/адаптирани препоръки (skill-ът не отменя проектните решения):** предложената cyan/healthcare палитра — отхвърлена (риск от "медицински стерилен вид", изрично забранено); заменена със самостоятелно синтезирана топла неутрална палитра + приглушен teal primary. Предложеният "Pattern: Social Proof-Focused, CTA above fold" (маркетингов landing page框) — не приложен буквално (платформата не използва social proof/агресивни CTA конвенции); Lora+Raleway wellness typography — заменен с по-четим/достъпен избор.
  - `ponytail:ponytail` (пасивно) — без external UI library (без Tailwind/Bootstrap/shadcn), собствена малка CSS основа; component wrapper Razor компонент създаден само за `DisclaimerCallout` (реална нужда от reuse на идентичен текст), не за бутони/карти (само CSS класове, без нужда от wrapper компонент).
  - `run` — реални build/test/HTTP smoke test изпълнения.
  - `security-review` — **не е приложен формално**: няма нови зависимости, secrets, auth, или потребителски вход в тази стъпка.
- **Защо са избрани:** `ui-ux-pro-max` — изрично задължителен skill за UI/UX стъпки по постоянно правило от `01_MASTER_PLAN.md`; `ponytail` управлява решението да не се добавя UI framework.
- **Засегнати файлове:** `app.css`, `MainLayout.razor`, `MainLayout.razor.css`, `App.razor`, `Home.razor`, `NotFound.razor`, `DisclaimerCallout.razor` (нов), `DesignSystemTests.cs` (нов).
- **Проверки:** build (0/0), тестове (6/6), реален HTTP smoke test на сервирания HTML/CSS.
- **Отклонения от skill инструкциите:** няма — skill-ът е ползван правилно като searchable reference, не като автоматичен генератор без преглед.
- **Резултат:** STEP-2.1 `COMPLETE`. `ui-ux-pro-max` доказано полезен, но изисква ръчна преценка за всяка препоръка спрямо клиничните/тоналните ограничения на проекта — това ще се прилага и занапред.

---

## 2026-08-01 — STEP-2.2: Real page wireframes and navigation

- **Roadmap step:** STEP-2.2 (`24_IMPLEMENTATION_ROADMAP.md`, ново добавен).
- **Използвани skills:**
  - `ui-ux-pro-max` (SKILL-046) — **основен skill**, задължителен. Реални CLI извиквания:
    1. `search.py "educational course catalog hero non-promotional" --domain landing -n 4` — 4 landing patterns.
    2. `search.py "card list catalog navigation" --domain ux -n 6` — navigation/accessibility правила.
  - **Приложени правила:** структурна логика "Hero + Features + CTA" (без цветовата му стратегия); `nav-state-active` (реализирано чрез Blazor `NavLink`, не ръчен CSS/JS).
  - **Отхвърлени препоръки (документирано защо):** vibrant/high-contrast marketing цветова схема — остава приглушената STEP-2.1 палитра; testimonials/social-proof/video-hero модели — забранени по продуктови правила (без измислени оценки/потребители/успехи); sticky navigation — ненужна сложност; breadcrumbs — преждевременно за 2-страничен сайт.
  - `ponytail:ponytail` (пасивно) — `ModuleCard` компонент създаден само защото реално намалява дублиране (2 идентични карти на 2 страници); не са добавени bUnit/browser testing frameworks, не е добавен UI framework.
  - `run` — реални build/test/HTTP smoke test изпълнения.
  - `security-review` — **не е приложен формално**: без нови зависимости/secrets/auth/потребителски вход.
- **Защо са избрани:** `ui-ux-pro-max` — изрично задължителен за UI стъпки; `ponytail` управлява решението кога компонент реално заслужава extraction.
- **Засегнати файлове:** `Home.razor`, `Programa.razor` (нов), `ModuleCard.razor` (нов), `MainLayout.razor`, `app.css`, `_Imports.razor`, `PublicPagesTests.cs` (нов).
- **Проверки:** build (0/0), тестове (9/9), реален HTTP smoke test с структурни assertions (h1 count, active nav state, disabled module CTA, disclaimer текст, липса на placeholder/dead links).
- **Отклонения от skill инструкциите:** няма.
- **Резултат:** STEP-2.2 `COMPLETE`. Phase 2 критерий за завършване изпълнен.

---

## 2026-08-01 — STEP-3.1: Първи реален обучителен content slice

- **Roadmap step:** STEP-3.1 (`24_IMPLEMENTATION_ROADMAP.md`, ново добавен, Фаза 3).
- **Използвани skills:**
  - `ui-ux-pro-max` (SKILL-046) — за структурата на образователната страница. Реални CLI извиквания:
    1. `search.py "long form educational article progressive disclosure reading" --domain ux -n 6` — line-length (потвърди вече съществуващия `--content-max` token), truncation/forms (нерелевантни).
    2. `search.py "callout notice box information" --domain style -n 2` — Bento Box Grid, Data-Dense Dashboard — **и двете отхвърлени**, не подхождат на спокоен образователен урок.
  - **Приложено:** line-length ограничение (вече token-базирано); прогресивно разкриване чрез heading йерархия (обяснение → пример → callout → проверка → обобщение), не нова skill препоръка, а естествена структура на урочния шаблон REQ-CONT-002.
  - Skill-ът не промени клиничното съдържание, не добави неподкрепени твърдения, не отмени design system решенията от STEP-2.1.
  - `ponytail:ponytail` (пасивно) — `LearningObjectives`/`SourceReferences` извлечени само защото реално се използват 2×; `TestPaths.cs` helper извлечен от `DesignSystemTests.cs` по същия принцип (2-ра реална употреба).
  - `run` — реални build/test/HTTP smoke test изпълнения (2 рунда — вторият след откриване и коригиране на disclaimer бъга).
  - `security-review` — **не е приложен формално**: без нови зависимости/secrets/auth/потребителски вход.
- **Content governance (не Claude Code skill, но задължителен процес за тази стъпка):** приложен изцяло преди писане — REQ/SRC идентифициране, изключване на непроверено съдържание (REQ-CLIN-008/009), маркиране `REQUIRES PROFESSIONAL REVIEW`.
- **Защо са избрани:** `ui-ux-pro-max` — изрично задължителен за UI/образователни страници; `ponytail` — управлява кога компонент/helper реално заслужава extraction; `run` — единствен начин да се докаже честно, че съдържанието реално работи (и разкри реален бъг).
- **Засегнати файлове:** `Kpt.razor`, `Modul2.razor`, `Modul2Lesson1.razor`, `LearningObjectives.razor`, `SourceReferences.razor`, `Home.razor`, `Programa.razor`, `app.css`, `ContentSliceTests.cs`, `TestPaths.cs`, `DesignSystemTests.cs`.
- **Проверки:** build (0/0), тестове (21/21), реален HTTP smoke test с структурни assertions — включително откриване и коригиране на реален compliance бъг (липсващ disclaimer).
- **Отклонения от skill инструкциите:** няма.
- **Резултат:** STEP-3.1 `COMPLETE`. Content governance процесът (не skill) беше основният определящ фактор за съдържанието — `ui-ux-pro-max` допринесе структурно, но клиничните граници идват изцяло от проектните документи.

---

## 2026-08-01 — STEP-3.2: Втори реален урок от Модул 2

- **Roadmap step:** STEP-3.2 (`24_IMPLEMENTATION_ROADMAP.md`).
- **Използвани skills:**
  - `ui-ux-pro-max` (SKILL-046) — за lesson hierarchy/четивна дължина. Реални CLI извиквания: `search.py "long form educational article progressive disclosure reading" --domain ux -n 6` (line-length потвърждение), `search.py "callout notice box information" --domain style -n 2` (Bento/Dashboard — отхвърлени).
  - **Запазена изцяло одобрената дизайн система** — без нова палитра, шрифт, dashboard оформление, bento grid, игровизация, badges, статистики.
  - `ponytail:ponytail` (пасивно) — не е създаден нов reusable компонент (`LearningObjectives`/`SourceReferences`/`ModuleCard` от преди вече покриват нуждата); нов урок = ново съдържание в established шаблон, не нова инфраструктура.
  - `run` — реални build/test/HTTP smoke test изпълнения; открита и документирана техническа особеност (HTML entity encoding на интерполирано кирилско съдържание) чрез реална Python декодираща проверка, не просто приета на доверие.
  - `security-review` — **не е приложен формално**: без нови зависимости/secrets/auth/потребителски вход.
- **Content governance (задължителен процес, не skill):** приложен изцяло — SRC-041 Гл. 9 директно подкрепя урока; "горещи когниции" терминология съзнателно опростена/пропусната за лаик аудитория.
- **Защо са избрани:** `ui-ux-pro-max` — задължителен за образователни страници; `ponytail` потвърди, че не е нужна нова инфраструктура (втори урок = доказателство, че шаблонът от STEP-3.1 реално се повтаря); `run` — единствен начин да се провери реално, включително откриване на не-очевидна техническа особеност.
- **Засегнати файлове:** `Modul2Lesson2.razor` (нов), `Modul2.razor`, `Modul2Lesson1.razor`, `ContentSliceTests.cs`.
- **Проверки:** build (0/0), тестове (25/25), реален HTTP smoke test с HTML-entity-decode верификация на source citation.
- **Отклонения от skill инструкциите:** няма.
- **Резултат:** STEP-3.2 `COMPLETE`. Вторият урок потвърждава, че установеният шаблон от STEP-3.1 реално се преизползва без нужда от нова инфраструктура — добър знак за бъдещото Markdown/JSON pipeline решение, но все още под препоръчания праг от 3–4 урока.

---

## 2026-08-01 — STEP-3.3: Трети урок и Content Pipeline Decision Gate

- **Roadmap step:** STEP-3.3 (`24_IMPLEMENTATION_ROADMAP.md`).
- **Използвани skills:**
  - `ui-ux-pro-max` (SKILL-046) — потвърждение, че дизайн системата остава непроменена (без нова палитра/шрифт/dashboard/gamification); не са правени нови CLI заявки тази стъпка — предходните (STEP-3.1/3.2) вече бяха достатъчни за established lesson pattern.
  - `ponytail:ponytail` (пасивно) — приложен буквално в Content Pipeline Decision Analysis: избран `KEEP RAZOR FOR MVP` именно защото Markdown/JSON pipeline би добавил инфраструктурна сложност (custom block syntax за `.card`/`.callout`) без пропорционална стойност при таван от 2-4 урока в единствения модул, който в момента реално има съдържание — учебникарски пример за "не изграждай инфраструктура преди доказана нужда".
  - `run` — реални build/test/HTTP smoke test изпълнения.
  - `security-review` — **не е приложен формално**: без нови зависимости/secrets/auth/потребителски вход.
- **Content governance + архитектурна реконсилиация (задължителен процес, не skill):** формален ADR-009 създаден за структурното решение (Модул 2 поема базовото ниво на Модул 3/5 теми); `18_INFORMATION_ARCHITECTURE.md` анотиран, за да не остане документацията в противоречие с реализацията — коригиран реален пропуск от STEP-3.1/3.2 (решението не беше формално записано в IA).
- **Защо са избрани:** `ponytail` директно определи изхода на pipeline анализа (не просто "consulted", а решаващ фактор); content governance процесът беше основният определящ фактор за архитектурната реконсилиация — не skill.
- **Засегнати файлове:** `Modul2Lesson3.razor` (нов), `Modul2.razor`, `Modul2Lesson2.razor`, `ContentSliceTests.cs`, `18_INFORMATION_ARCHITECTURE.md`, `03_DECISION_LOG.md`.
- **Проверки:** build (0/0), тестове (30/30), реален HTTP smoke test.
- **Отклонения от skill инструкциите:** няма.
- **Резултат:** STEP-3.3 `COMPLETE`. Content Pipeline решение: `KEEP RAZOR FOR MVP`, окончателно за текущия обхват на флагманския модул.

---

## 2026-08-01 — STEP-3.4: Модул 1 overview и първи реален урок

- **Roadmap step:** STEP-3.4 (`24_IMPLEMENTATION_ROADMAP.md`).
- **Използвани skills:**
  - `ui-ux-pro-max` (SKILL-046) — потвърдено приложимо. Един targeted CLI check: `search.py "module overview list progressive disclosure" --domain ux -n 4` → 0 нови резултата — потвърждава, че вече прочетените правила от STEP-3.1–3.3 (line-length, callout placement, nav active-state, card patterns) са достатъчни; не са направени излишни повторни заявки, съгласно изричната инструкция.
  - Дизайн системата запазена изцяло непроменена (топла палитра, teal, Atkinson Hyperlegible, tokens, focus-visible, reduced-motion) — без dashboard/bento/gamification/marketing.
  - `ponytail:ponytail` (пасивно) — Модул 1 реализиран като единствен урок (честно, по IA), не изкуствено разделен на няколко, за да "прилича" на Модул 2; никакъв нов reusable компонент не е създаден (съществуващите четири напълно достатъчни).
  - `run` — реални build/test/HTTP smoke test изпълнения.
  - `security-review` — **не е приложен формално**: без нови зависимости/secrets/auth/потребителски вход.
- **Content governance (задължителен процес, не skill):** REQ-CLIN-009 приложен внимателно — само общата, безопасна evidence-base claim, идентично ограничение като STEP-3.1's `/kpt`.
- **Защо са избрани:** `ui-ux-pro-max` — задължителен за UI/образователни страници, но приложен ефективно без прекомерни повторни заявки; `ponytail` — предпази от изкуствено разширяване на Модул 1 отвъд одобрения му, едноурочен обхват.
- **Засегнати файлове:** `Modul1.razor` (нов), `Modul1Lesson1.razor` (нов), `Programa.razor`, `Home.razor`, `ContentSliceTests.cs`.
- **Проверки:** build (0/0), тестове (38/38), реален HTTP smoke test — вкл. потвърждение, че 0 disabled карти остават на публичните страници.
- **Отклонения от skill инструкциите:** няма.
- **Резултат:** STEP-3.4 `COMPLETE`. Content Pipeline решението (`KEEP RAZOR FOR MVP`) потвърдено отново приложимо — Модул 1 се вписа безпроблемно в established модела.

---

## 2026-08-01 — Visual Direction Correction: dark-first UI + интерактивни учебни визуализации

- **Roadmap step:** извънреден checkpoint (собственически визуален review gate), не номериран STEP от `24_IMPLEMENTATION_ROADMAP.md`.
- **Използвани skills:**
  - `ui-ux-pro-max` (SKILL-046) — **основен skill**, задължителен за тази стъпка, с целенасочени dark-theme/accessibility заявки. Реални CLI извиквания:
    1. `search.py "dark mode not pure black elevated surfaces" --domain style -n 4` → намерена "Dark Mode OLED" (pure-black/neon) — **отхвърлена**, изрично забранена от собственическата инструкция (not-pure-black изискване).
    2. `search.py "dark mode calm educational readable" --domain color -n 4` → намерена "Modern Dark Cinema Mobile" (glassmorphism/blur/glow/haptics) — **отхвърлена**, декоративни ефекти извън обхвата (само функционални преходи).
    3. `search.py "accessible focus states keyboard contrast" --domain ux -n 6` — потвърждение на focus-visible/keyboard/aria-live правила, приложени директно.
    4. `search.py "diagram interactive step by step visualization" --domain ux -n 4` — потвърждение на progressive disclosure/native `<details>` fallback подхода за диаграмата.
  - **Приложено, не механично прието:** и двата отхвърлени "dark" стила доведоха до самостоятелно синтезирана multi-level dark палитра (background/surface/elevated-surface, без чист черен, без blur/glow), реално проверена с WCAG contrast математика (Python скрипт), не skill препоръка на доверие.
  - `ponytail:ponytail` (пасивно) — `--color-link` token добавен само защото един hex не можеше да удовлетвори едновременно text-role (≥4.5:1) и fill-role (≥3:1) contrast; без external chart/diagram библиотека за `CbtModelDiagram` (native buttons + CSS); theme toggle с 5-редов JS interop вместо framework/library.
  - `run` — реални build/test/HTTP smoke test изпълнения (2 рунда — вторият след откриване и коригиране на orphaned-process/stale-port проблем чрез PowerShell).
  - `security-review` — **не е приложен формално**: без нови зависимости/secrets/auth/server-side storage; theme state изрично сесийно (in-memory), без cookies/localStorage.
- **Content governance:** не приложимо — клинично съдържание непроменено, thought record не е започнат (изрично забранено).
- **Защо са избрани:** `ui-ux-pro-max` — изрично задължителен и за целенасочените dark-theme заявки, но и двете предложени "dark" стилови references бяха съзнателно отхвърлени заради несъвместимост с изричните собственически ограничения (not-pure-black, no decorative effects) — skill-ът послужи като starting point за проучване, не като краен резултат; `ponytail` управлява минималния обхват на новите токени/компоненти; `run` — единствен начин да се докаже честно WCAG съответствие и реално prerendered WASM markup, а не просто HTTP 200.
- **Засегнати файлове:** `app.css`, `App.razor`, `MainLayout.razor`, `_Imports.razor`, `theme.js` (нов), `ThemeToggle.razor` (нов), `CbtModelDiagram.razor` (нов), `InterpretationExample.razor` (нов), `LearningPathVisualization.razor` (нов), `Kpt.razor`, `Modul2Lesson1.razor`, `Home.razor`, `Programa.razor`, `InteractiveUiTests.cs` (нов), `ContentSliceTests.cs`.
- **Проверки:** build (0/0), тестове (56/56), реален HTTP smoke test на fresh инстанция (след коригиране на stale-process проблема) — `data-theme="dark"` на initial HTML, всички нови компонентни маркери, prerendered WASM markup потвърден, light override присъства.
- **Отклонения от skill инструкциите:** няма — двете отхвърлени препоръки са документирани изрично с причина, не мълчаливо игнорирани.
- **Резултат:** технически завършено, `TECHNICAL QA COMPLETE — OWNER VISUAL APPROVAL PENDING`. Некомитнато — изчаква собственически визуален преглед.

---

## 2026-08-01 — Weekly Course Hub + Simulator Foundation (ADR-010)

- **Roadmap step:** извънреден checkpoint, натрупан върху Visual Direction Correction checkpoint-а, не номериран STEP — виж `24_IMPLEMENTATION_ROADMAP.md`.
- **Използвани skills:**
  - `ui-ux-pro-max` (SKILL-046) — **основен skill**, задължителен. Реални CLI извиквания:
    1. `search.py "weekly course hub sidebar navigation" --domain ux -n 4` — sticky nav padding, keyboard navigation, back-button history (приложено принципно — Blazor `NavLink`/browser back вече го дава native), breadcrumbs — **отхвърлено** (преждевременно за текущия мащаб на сайта).
    2. `search.py "dark educational portal application shell" --domain style -n 4` → "Dark Mode (OLED)" (pure-black/neon) и "Modern Dark (Cinema Mobile)" (glassmorphism/blur/glow/haptics) — **и двете отхвърлени отново**, идентично на Сесия 17 — потвърждава консистентност на решението през две отделни стъпки, не случаен избор. Cyberpunk UI — отхвърлен (neon gaming, изрично забранено). Storytelling-Driven — отхвърлен (scroll-triggered reveals/parallax, конфликтира с "no auto-play/parallax").
    3. `search.py "progressive disclosure long form learning collapsible" --domain ux -n 4` — truncation-with-expand (не приложено буквално — native `<details>` предпочетен пред line-clamp+expand за случая), form/line-length правила (нерелевантни/вече приложени).
    4. `search.py "accessible collapsible content details summary" --domain ux -n 3` — ARIA labels за icon-only бутони (нерелевантно — няма icon-only бутони тук), keyboard navigation (потвърдено, native).
    5. `search.py "knowledge depth levels cognitive load reduction" --domain ux -n 3` → VisionOS depth layering (glass material) — **отхвърлено**, напълно неприложимо за web Blazor и директно конфликтира с "no blur" решението от Сесия 17; lazy loading/image optimization — нерелевантни (няма изображения в тази стъпка).
  - **Собственически reference:** `https://logicraft-portal.netlify.app/` (предоставен директно от собственика в отговор на моя grounding проверка, не измислен/предположен) — fetched чрез WebFetch, извлечена **само структурна** информация (sidebar групи "Simulators & Tools"/"Revision & Practice", top bar, weekly hub с collapsible седмици, card-based simulator layout). Изрично НЕ пренесени: branding, light цветова схема, AI Tutor/Copilot секция, gamification елементи (streak/progress badges) — всички директно забранени в самата инструкция.
  - `ponytail:ponytail` (YAGNI) — sidebar mobile fallback е обикновен document-flow reorder (без hamburger/JS drawer) — списъкът е кратък (6 елемента), JS drawer би бил ненужна сложност; `ProgressiveExplanation` е single-purpose native `<details>` wrapper, не compound multi-part компонент; `CourseWeekStatus` винаги се извежда от `SafetyLevel`+`Route` (`DeriveStatus()` helper), никога ръчно дублиран — предотвратява клас бъгове от разсинхронизация, не просто "по-малко код".
  - `run` — build/test/HTTP smoke test изпълнения; орфан процес на порт 5055 открит отново и коригиран точково чрез PowerShell (не блокетен `dotnet` kill) преди smoke test.
  - `security-review` — **не е приложен формално**: без нови зависимости/secrets/auth/server-side storage; `CategorizationCheck` изрично проектиран без сравнение "избор на потребителя срещу верен отговор" (reveal-style), за да изключи всякаква форма на скрито оценяване по дизайн, не само по декларация.
- **Content governance + PDF grounding (задължителен процес, не skill):** `kpt_syllabus.pdf` прочетен изцяло преди всякаква имплементация; кръстосан срещу вече зададената от собственика safety classification (потвърдено съответствие) и срещу вече заключения (`ADR-005`) SRC-041 REQ-CLIN-001 distortion списък (открито и докладвано реално разминаване за Седмица 9 — PDF-ов частичен списък не заменя citation-grade източника).
- **Защо са избрани:** `ui-ux-pro-max` — задължителен, но приложен селективно — множество препоръки (2 dark стила, VisionOS depth, storytelling reveals) отхвърлени с изрична причина, не механично приети; `ponytail` директно определи sidebar mobile подхода и `DeriveStatus()` дизайна; `run` — единствен начин да се докаже честно, че всички 15 week записа и Седмица 8 реално рендерират коректно, включително откриване на поредния orphan-process проблем.
- **Засегнати файлове:** `MainLayout.razor`, `app.css`, `_Imports.razor`, `ContentSliceTests.cs`, `InteractiveUiTests.cs`, `Curriculum/CurriculumEnums.cs` (нов), `Curriculum/CourseCatalog.cs` (нов), `Kurs.razor` (нов), `Sedmica8.razor` (нов), `ProgressiveExplanation.razor` (нов), `CategorizationCheck.razor` (нов), `CurriculumHubTests.cs` (нов).
- **Проверки:** build (0/0), тестове (81/81), реален HTTP smoke test (всички routes, 15 week-list елемента, prerendered interactive markup, без ECTS/институционални claims, без forbidden distortion string).
- **Отклонения от skill инструкциите:** няма — всяко отхвърляне е документирано с изрична причина.
- **Резултат:** технически завършено, `WEEKLY HUB AND SIMULATOR FOUNDATION READY — OWNER VISUAL REVIEW REQUIRED`. Некомитнато, натрупано върху некомитнатата Сесия 17 работа — изчаква собственически визуален преглед на двата checkpoint-а заедно.

---

## 2026-08-03 — Content-Rich Simulator Redesign (собственически визуален отказ #2)

- **Roadmap step:** "Redesign Round 2" checkpoint — виж `24_IMPLEMENTATION_ROADMAP.md`.
- **Използвани skills:**
  - `ui-ux-pro-max` (SKILL-046) — targeted заявки: `search.py "interactive simulator control panel live output" --domain ux -n 4` (hover states/input affordance/disabled states/ARIA icon-labels — приети принципно, вече покрити от съществуващите `.btn`/`.field` design tokens, не изискваха нов код); `search.py "diagram led sectioned learning dark laboratory" --domain style -n 4` → OLED, Modern Dark Cinema Mobile, Cyberpunk, Claymorphism — **всичките четири отхвърлени**. Claymorphism е ново отхвърляне тази сесия (playful/bubbly/gradient/haptic естетика за deti/gamified продукти — директно противоречи на спокойния, сериозен образователен тон на платформата и на изричната "no gamification" забрана).
  - `ponytail:ponytail` (YAGNI, root-cause) — телесната реакция в `CbtChainSimulator` е **изведена** от избраната емоция (dictionary lookup), не отделен 6-и select контрол — по-малко UI повърхност, същата педагогическа връзка "емоция↔телесна реакция", без имплициране, че двете са независимо избираеми факти; `.workspace` контейнер добавен само защото реално диаграмите/симулаторът се нуждаеха от повече ширина от съществуващия `.content` четивен стълб — не преработен целият layout system.
  - `run` — build/test/HTTP smoke test; 2 реални test failures открити и коригирани (буквалната дума "score" в собствен код коментар; остарял тест все още очакващ `CbtModelDiagram` на Седмица 8 вместо новия `CbtChainSimulator`) — и двете бяха самонаправени грешки от самия редизайн, не пропуски в тестовата логика, коригирани веднага след `dotnet test` разкри ги.
  - `security-review` — **не е приложен формално**: без нови зависимости/secrets/auth/server-side storage; `CbtChainSimulator` изрично проектиран без `<textarea>`/`type="text"` (без личен свободен текст), тествано автоматично.
- **Content governance (задължителен процес, не skill):** "Optional visual hierarchy" (автоматична мисъл→правило→основно вярване, Section 11.D от инструкцията) съзнателно пропуснато — Модул 2 не преподава междинни/основни вярвания (бъдещи Седмица 11/12 теми по curriculum classification-а от Сесия 18); включването ѝ би въвело content извън одобрения обхват без отделен review.
- **Защо са избрани:** `ui-ux-pro-max` — задължителен, но отново основната стойност беше в кое се отхвърля (4 поредни decorative/dark стила несъвместими с проекта), не в кое се приема буквално; `ponytail` определи телесна-реакция-като-derived-not-selected дизайна и `.workspace` scope; `run` — единствен начин да се докаже честно, че редизайнът реално работи, включително откриване на 2 реални собствени грешки.
- **Засегнати файлове:** `InterpretationExample.razor`, `CategorizationCheck.razor`, `Sedmica8.razor`, `Kurs.razor`, `app.css`, `CurriculumHubTests.cs`, `InteractiveUiTests.cs`.
- **Създадени файлове:** `CbtChainSimulator.razor`, `SectionArchitectureTests.cs`.
- **Проверки:** build (0/0), тестове (107/107 — след коригиране на 2 реални failures), реален HTTP smoke test (8 section anchors, всички select опции реално prerendered, branch diagram/comparison matrix/course map/timeline реално рендерени).
- **Отклонения от skill инструкциите:** няма — всяко отхвърляне документирано с изрична причина.
- **Резултат:** технически завършено, `CONTENT-RICH SIMULATOR REDESIGN READY — OWNER VISUAL REVIEW REQUIRED`. Некомитнато, трети натрупан checkpoint върху некомитнатата Сесия 17+18 работа.

---

## 2026-08-03 — Two-Column Workspace and Semantic Color (собственически визуален отказ #3)

- **Roadmap step:** "Redesign Round 3" checkpoint — виж `24_IMPLEMENTATION_ROADMAP.md`.
- **Използвани skills:**
  - `ui-ux-pro-max` (SKILL-046) — `search.py "two column educational workspace split layout" --domain ux -n 4` (generic layout best practices — content jumping/fixed positioning/stacking context/viewport units — вече принципно спазени, без нужда от нов код); `search.py "semantic color coded learning sections dark interface" --domain color -n 4` → 4 готови продуктови палитри (Quantum Computing/Kids Learning/Language Learning/Music Instrument) — **нито една копирана буквално**: Quantum Computing (neon cyan/purple) отхвърлен, Kids Learning (bright/playful) отхвърлен, Music Instrument (warm red, грешна доменна асоциация) отхвърлен; Language Learning App (индиго + WCAG-коригиран зелен) използван само за **принципна** насока (ограничен брой hues, един primary + accent) — точните hex стойности изчислени самостоятелно за нашата база, не заимствани.
  - `ponytail:ponytail` (reuse преди нов код) — `--accent-interactive`/`--accent-example`/`--accent-safety` **alias-нати** към вече верифицираните teal/amber/rose tokens от Сесия 17, вместо да се преизчисляват нови стойности; `.btn-primary`/`.btn-secondary` (teal) оставени изцяло непроменени навсякъде другаде — само добавен нов `.btn-violet` вариант там, където инструкцията изрично поиска различен primary color, вместо да се преоцвети целият сайт; sticky sidebar съзнателно пропуснат (условно разрешен от инструкцията, но добавената сложност/accessibility риск не си заслужаваше за 1 кратък sidebar).
  - `run` — build/test/HTTP smoke test; орфан процес на порт 5055 отново идентифициран по PID и спрян точково преди рестарт.
  - `security-review` — **не е приложен формално**: чисто CSS/layout промяна, без нови зависимости/входни данни.
- **Content governance:** не приложимо — само визуален/layout refinement, клинично съдържание непроменено.
- **Защо са избрани:** `ui-ux-pro-max` — задължителен, но резултатите бяха предимно generic/неприложими тази стъпка (различно от предходните сесии, където отхвърлянето на конкретни dark стилове носеше директна стойност) — реалната работа тук беше собствено WCAG изчисление, не skill lookup; `ponytail` директно определи кои токени да се alias-нат вместо преизчислят, и решението да не се строи sticky sidebar.
- **Засегнати файлове:** `Sedmica8.razor`, `Kurs.razor`, `CbtChainSimulator.razor`, `DisclaimerCallout.razor`, `app.css`.
- **Създадени файлове:** `LayoutRefinementTests.cs`.
- **Проверки:** build (0/0), тестове (120/120), реален HTTP smoke test (learning-grid редове, section-card роли, accent tokens, btn-violet, timeline+sidebar split — всички потвърдени в реално сервирания HTML/CSS).
- **Отклонения от skill инструкциите:** няма.
- **Резултат:** технически завършено, `TWO-COLUMN COLOR-RICH WORKSPACE READY — OWNER VISUAL REVIEW REQUIRED`. Некомитнато, четвърти натрупан checkpoint върху некомитнатата Сесия 17+18+19 работа.

---

## 2026-08-04 — Global Two-Column Redesign Across All Existing Routes (собственически визуален отказ #4, със screenshot)

- **Roadmap step:** "Redesign Round 4" checkpoint — виж `24_IMPLEMENTATION_ROADMAP.md`.
- **Използвани skills:**
  - `ui-ux-pro-max` (SKILL-046) — **съзнателно не използван за нови заявки тази стъпка**. Предходните 4 сесии вече изчерпателно покриха dark theme/split layout/semantic color/simulator пространството; допълнителни заявки биха били механично повторение без нова стойност, а не реално прилагане на skill discipline. Решението е документирано изрично в `10_SESSION_LOG.md`, не пропуснато мълчаливо.
  - `ponytail:ponytail` (root-cause, YAGNI) — приложен буквално и показателно: вместо поредна CSS "лепенка" (напр. специфичен fix само за `Sedmica8.razor`), диагностициран и коригиран реалният root cause (`.content` без `margin: auto`, скрит от 4 сесии, докато родителският контейнер не стана достатъчно широк, за да го направи видим); container-query решението (вместо поредния viewport breakpoint) адресира правилния проблем (sidebar изяжда ширина) вместо симптома; lesson шаблонът калибриран до 3 реда вместо форсираните 4 от инструкцията, за да не се измисля ново съдържание само за да запълни структура.
  - `run` — build/test/HTTP smoke test на всичките 11 routes; орфан процес на порт 5055 отново идентифициран по PID и спрян точково.
  - `security-review` — **не е приложен формално**: чисто CSS/layout/markup промяна, без нови зависимости/входни данни.
- **Content governance:** Модул 1 Урок 1 добави нова визуализация (сравнителна таблица образование-срещу-терапия), но тя е буквална преформулировка на вече одобрения disclaimer текст — не нова клинична claim, не изисква нов source review.
- **Защо са избрани:** `ponytail` беше решаващият фактор за целия подход тази стъпка — от диагностиката на CSS бъга до калибрирането на lesson шаблона; `run` — единствен начин да се докаже честно, че двуколонният pattern реално работи на всичките 11 routes, не само на 2-те вече редизайнати.
- **Засегнати файлове:** `Home.razor`, `Programa.razor`, `Kpt.razor`, `Modul1.razor`, `Modul1Lesson1.razor`, `Modul2.razor`, `Modul2Lesson1.razor`, `Modul2Lesson2.razor`, `Modul2Lesson3.razor`, `ModuleCard.razor`, `MainLayout.razor`, `CategorizationCheck.razor`, `CbtChainSimulator.razor`, `app.css`.
- **Създадени файлове:** `LearningSection.razor`, `GlobalRedesignTests.cs`.
- **Проверки:** build (0/0), тестове (159/159), реален HTTP smoke test на всичките 11 routes (реален `.learning-grid` count на всяка страница, не само presence check).
- **Отклонения от skill инструкциите:** няма.
- **Резултат:** технически завършено, `GLOBAL TWO-COLUMN LEARNING WORKSPACE READY — OWNER VISUAL REVIEW REQUIRED`. Некомитнато, пети натрупан checkpoint върху некомитнатата Сесия 17+18+19+20 работа.

---

## 2026-08-04 — Final Visual Polish (навигация/CTA/visual models/text density)

- **Roadmap step:** "Final Visual Polish" checkpoint — виж `24_IMPLEMENTATION_ROADMAP.md`.
- **Използвани skills:**
  - `ui-ux-pro-max` (SKILL-046) — **не използван за нови заявки тази стъпка**. Чисто изпълнителска polish стъпка върху вече установени design decisions от предходните 5 сесии — реален sidebar bug fix и progressive disclosure прилагане не изискват ново UI/UX проучване, само коректно прилагане на вече взети решения.
  - `ponytail:ponytail` (root-cause) — приложен буквално върху sidebar bug-а: вместо да се добавя custom CSS override или JS logic, за да се "поправи" визуалния симптом (два active елемента), диагностициран истинският root cause (липсващ `NavLinkMatch.All`) и коригиран с вградената Blazor функционалност — 1-редова поправка на линк, не нова инфраструктура. Активното pressed-състояние на бутоните (`filter: brightness(0.9)`) добавено генерично веднъж на базовия `.btn` клас, не дублирано за всеки вариант.
  - `run` — build/test/HTTP smoke test на всичките 11 routes; орфан процес на порт 5055 отново идентифициран по PID и спрян точково.
  - `security-review` — **не е приложен формално**: чисто CSS/markup polish, без нови зависимости/входни данни.
- **Content governance:** нови "Граници" изречения добавени във всяко пълно обяснение (4-те урока) — всичките са преформулировки/уточнения на вече одобрено съдържание (напр. "връзката емоция-тяло не е универсална"), не нови клинични claims.
- **Защо са избрани:** `ponytail` беше решаващият фактор за sidebar bug fix-а — точков root-cause fix вместо CSS патч; `run` — единствен начин да се докаже честно, че активната навигация вече показва точно 1 елемент (не просто "изглежда поправено" визуално).
- **Засегнати файлове:** `MainLayout.razor`, `Home.razor`, `Modul1Lesson1.razor`, `Modul2Lesson1.razor`, `Modul2Lesson2.razor`, `Modul2Lesson3.razor`, `CbtModelDiagram.razor`, `app.css`.
- **Създадени файлове:** `FinalPolishTests.cs`.
- **Проверки:** build (0/0), тестове (185/185), реален HTTP smoke test (active-nav count, CTA/learning-path markup, progressive-explanation markers на 4-те урока, нов dark background hex, gutter clamp — всички потвърдени в реално сервирания HTML/CSS).
- **Отклонения от skill инструкциите:** няма.
- **Резултат:** технически завършено, `FINAL VISUAL POLISH READY — OWNER APPROVAL REQUIRED`. Некомитнато, шести натрупан checkpoint върху некомитнатата Сесия 17–21 работа.

---

## 2026-08-04 — Final Layout Defect Correction (overflow/focus/module density/hierarchy)

- **Roadmap step:** "Final Layout Defect Correction" checkpoint — виж `24_IMPLEMENTATION_ROADMAP.md`.
- **Използвани skills:**
  - `ui-ux-pro-max` (SKILL-046) — **не използван за нови заявки тази стъпка**. Всичките 12 доклада бяха или конкретни, диагностицируеми CSS/Blazor root-cause bug-ове (overflow, focus ring), или точкови polish детайли — не изискваха ново UI/UX проучване, изискваха реален debugging.
  - `ponytail:ponytail` (root-cause, не symptom) — буквално приложен два пъти: (1) overflow-бъгът — вместо `overflow-x: hidden` workaround (изрично забранен и в двата случая би бил грешен избор), диагностицирана точната CSS grid причина (`min-width: auto` default) и коригирана с едно правило, което по съвпадение реши и втория blocking проблем (stretching) едновременно — двоен ефект от единствен root-cause fix; (2) focus рамката — вместо да се маха `outline` глобално (би счупило достъпността за реални клавиатурни потребители), диагностицирано точно кой Blazor механизъм (`<FocusOnNavigate>`) добавя `tabindex="-1"` и защо генеричното CSS правило го хваща, после добавено прецизно, скопирано изключение само за headings.
  - `run` — build/test/HTTP smoke test; открит и коригиран реален "умрял между стъпки" сървърен процес (различен клас проблем от предишните "надживял kill" случаи), плюс diagnostic находка за phantom TimeWait port записи (State колона разкри, че не блокират нов listener).
  - `security-review` — **не е приложен формално**: чисто CSS/markup/Blazor-attribute polish, без нови зависимости/входни данни.
- **Content governance:** module-path текстовете ("Основна идея и граници", кратки lesson функции) са преформулировки на вече одобрено съдържание, не нови клинични твърдения.
- **Защо са избрани:** `ponytail` беше решаващ и за двата blocking проблема — root-cause diagnostика вместо symptom suppression доведе директно до по-малки, по-точни CSS промени от очакваното; `run` — единствен начин да се потвърди честно, че overflow-ът реално изчезва и фокус рамката реално не се появява при нормално зареждане, не само "изглежда поправено" в кода.
- **Засегнати файлове:** `MainLayout.razor`, `ModuleCard.razor`, `DisclaimerCallout.razor`, `Home.razor`, `Kpt.razor`, `Programa.razor`, `Modul1.razor`, `Modul2.razor`, 4-те lesson файла, `Sedmica8.razor`, `app.css`, `LayoutRefinementTests.cs`.
- **Създадени файлове:** `LayoutDefectFixTests.cs`.
- **Проверки:** build (0/0), тестове (227/227), реален HTTP smoke test (module-path структура на двата модула, concept maps, ModuleCard без пълна рамка, "Наличен" отсъства, educational disclaimer variant, sidebar ширина, heading focus CSS, duplicate-label badge текстове отсъстват — всички потвърдени в реално сервирания HTML/CSS).
- **Отклонения от skill инструкциите:** няма.
- **Резултат:** технически завършено, `FINAL LAYOUT DEFECTS FIXED — OWNER APPROVAL REQUIRED`. Некомитнато, седми натрупан checkpoint върху некомитнатата Сесия 17–22 работа.
