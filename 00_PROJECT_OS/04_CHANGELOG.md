# 04 — Changelog

Формат: най-новото най-отгоре. Всеки запис отбелязва датата и типа промяна (добавено / променено / поправено / премахнато / архитектура / база данни / документация).

---

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
