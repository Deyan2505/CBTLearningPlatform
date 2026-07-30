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
