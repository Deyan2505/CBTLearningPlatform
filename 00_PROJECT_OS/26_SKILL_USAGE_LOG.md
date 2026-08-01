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
