# 02 — Current Status

*Актуализира се след всяка работна сесия. Единствен източник на истина за "къде сме сега".*

## ACTIVE CONTEXT FOR CURRENT STEP

Минимален набор документи, необходими за следващата стъпка (собственически content + визуален преглед → корекции → commit):

- `02_CURRENT_STATUS.md` (този файл — винаги първи).
- `24_IMPLEMENTATION_ROADMAP.md` → Slice 2 (Седмица 3) checkpoint запис.
- Локално стартираното приложение на `http://localhost:5055` (не е committed) — `/kurs/sedmica-3`.

**Не е необходимо** за рутинна техническа стъпка: Source Register/Coverage Matrix/Gaps (11–15), PRD (17), пълен Risk Register, пълен Session Log.

## Текуща фаза

Фаза 0 — завършена. Фаза 1 — STEP-1.1–1.5 `COMPLETE`; STEP-1.6 `DEFERRED`. Фаза 2 — STEP-2.1/2.2 `COMPLETE`. Фаза 3 — STEP-3.1–3.4 `COMPLETE`. **Foundation (Сесии 17–24) — `COMPLETE`, `COMMITTED` (hash `115f5fa`). Седмица 1 — `COMPLETE`, `COMMITTED` (Сесия 27). Седмица 3 + systemic route-safe anchor contract — `COMPLETE`, `COMMITTED` (Сесия 31).**

## Текуща стъпка

`WEEK 3 AND SYSTEMIC ANCHOR FIX COMMITTED` (Сесия 31). Собственикът одобри визуално и функционално Седмица 3, вътрешната anchor навигация на Седмици 1/3/8, глобалния skip-link и поправения section flow на Седмица 3 — изпълнен финален pre-flight (build/test/route smoke test/anchor navigation pre-flight/content pre-flight/layout pre-flight) и създаден единствен commit `feat: add week 3 cognitive model slice and route-safe anchors`. "Concept and Diagram" архетипът е `VALIDATED` — третият representative-week архетип на Weekly Course Hub-а реално доказа, че ADR-010 архитектурата мащабира и към йерархично/диаграмно съдържание. Systemic route-safe anchor contract-ът е `COMPLETE` — bare-fragment риска (потвърден в Седмица 1/3/8 и site-wide skip-link) е технически разрешен навсякъде и вече в history. **Ясно разграничение на текущия обхват:**

- **Visual/UX foundation** — `COMPLETE`, `COMMITTED` (hash `115f5fa`).
- **Weekly Course Hub foundation** — `COMPLETE`, `COMMITTED` (`/kurs` route, `CourseCatalog.cs`, `DeriveStatus()` — ADR-010).
- **Седмица 1 (Theory and History архетип)** — `COMPLETE`, `COMMITTED` (`/kurs/sedmica-1`) — архетипът `VALIDATED`.
- **Седмица 3 (Concept and Diagram архетип)** — `COMPLETE`, `COMMITTED` (`/kurs/sedmica-3`) — архетипът `VALIDATED`.
- **Седмица 8 (Simulator Workspace архетип)** — `COMPLETE`, `COMMITTED` (`/kurs/sedmica-8`).
- **Systemic route-safe anchor contract** — `COMPLETE`, `COMMITTED` (Седмица 1/3/8 section-nav + global skip-link, всички route-safe; whole-tree regression тест предпазва от бъдеща регресия).
- **Седмица 10 и останалите 11 седмици** — `NOT STARTED` (само метаданни в `CourseCatalog.cs`).
- **Независим академичен/клиничен review на съдържанието** — `PENDING` (RISK-010 — няма щатен рецензент; съдържанието не е публикувано за реални потребители извън локалната разработка).
- **Следваща фаза** — `CONTENT-DRIVEN TEMPLATE VALIDATION — WEEK 10` (име, не старт — изисква ново собственическо извикване; **не е започната автоматично**).

## Последна завършена задача

Week 3 Final Pre-Flight and Commit, включващ Systemic Anchor Fix (Сесия 31, 2026-08-06): собственикът одобри Седмица 3, anchor навигацията на Седмици 1/3/8, глобалния skip-link и поправения section flow; изпълнен пълен pre-flight (build/test, 13-route smoke test, anchor navigation pre-flight на трите седмици + skip-link на 6 routes, Week 3 content pre-flight, Week 3 layout pre-flight) и създаден единствен commit `feat: add week 3 cognitive model slice and route-safe anchors`. Виж `10_SESSION_LOG.md` (Сесия 31) за пълен commit hash и diffstat.

Systemic Route-Safe Anchor Fix (Сесия 30, 2026-08-06): систематичен одит и поправка на bare-fragment anchor риска, докладван в Сесия 29 — `Sedmica1.razor` (9 anchors), `Sedmica8.razor` (8 anchors) вече route-safe (`Sedmica3.razor` вече беше поправен). Site-wide skip-link в `MainLayout.razor` поправен динамично чрез `NavigationManager` (`{uri.AbsolutePath}{uri.Query}#main-content`), тъй като целевата страница се променя на всеки route. `<base href="/">` в `App.razor` — недокоснат (задължителен). Нов `SystemicAnchorFixTests.cs` с whole-tree regression тест, потвърждаващ 0 bare fragment hrefs остават никъде в `Components/`. 9 нови/актуализирани теста (общо 296/296 passing). **Некомитнато.** Пълни детайли в `10_SESSION_LOG.md` (Сесия 30).

WEEK 3 Owner Review — Anchor Navigation and Grid Gap Fix (Сесия 29, 2026-08-06): собственически преглед на живо на `/kurs/sedmica-3` докладва 2 blocking дефекта. Root cause #1: `App.razor`-овият `<base href="/">` кара bare `href="#id"` да резолва спрямо "/" (Home), не спрямо текущата страница — поправено само в Седмица 3 чрез пълни пътища; идентичен риск докладван, но **не поправен**, в Седмица 1/Седмица 8 и site-wide skip-link-а в `MainLayout`. Root cause #2: секции 08/09 бяха сдвоени в общ двуколонен ред с несъответстващи височини, оставяйки празна зона под по-късата — поправено чрез разделяне в собствени пълноширочинни секции. 2 нови regression теста (общо 287/287 passing). **Некомитнато.** Пълни детайли в `10_SESSION_LOG.md` (Сесия 29).

CONTENT-DRIVEN TEMPLATE VALIDATION — Slice 2: Седмица 3 (Сесия 28, 2026-08-06): изграден нов route `/kurs/sedmica-3` — третият representative-week архетип ("Concept and Diagram"), валидиращ че Weekly Course Hub архитектурата (ADR-010) реално мащабира към йерархично/диаграмно съдържание, различно и от двата предишни архетипа. Два нови interactive компонента (`CognitiveHierarchyExplorer` — reuse на `.cbt-diagram`; `SchemaFilterDemonstration` — нов малък `.schema-filter` CSS блок за toggle+list). Реконсилирани syllabus твърдения (основните вярвания не са задължително отрицателни; "схема като филтър" изрично обозначена като метафора; "автоматично" не се приравнява на "ирационално"; рефлексивната обработка не гарантира безгрешен резултат). 26 нови/актуализирани теста (общо 285/285 passing). **Некомитнато** — чака собственически преглед. Пълни детайли в `10_SESSION_LOG.md` (Сесия 28).

Week 1 Final Pre-Flight and Commit (Сесия 27, 2026-08-06): собственикът одобри съдържанието и визуалната реализация на Седмица 1; изпълнен пълен pre-flight (restore/build/test, 12-route smoke test, content pre-flight — заглавие/badge/reconciled формулировки/1979 цитат/learner-facing academic context/български review статус/educational disclaimer) и създаден единствен commit `feat: add week 1 CBT history learning slice`. Виж `10_SESSION_LOG.md` (Сесия 27) за пълен commit hash и diffstat.

CONTENT-DRIVEN TEMPLATE VALIDATION — Owner review correction (Сесия 26, 2026-08-06): собственически визуален преглед на Седмица 1 доведе до 9 точкови content-driven корекции — премахнат вътрешен development език от публичния HTML; нов scoped `.research-turn-stepper` responsive CSS Grid (споделеният `.cbt-diagram` остава непроменен); нов `HistoricalTimeline.Compact` density вариант (Course Hub timeline непроменена); нов sidebar "weak context" слой в `MainLayout`; скъсено заглавие; content-format badge вместо availability pill; преформулирана knowledge-check инструкция; Section 09 разбита на 3 подблока; Section 01 сведена до същината си. 10 нови теста (общо 258/258 passing). Пълни детайли в `10_SESSION_LOG.md` (Сесия 26).

CONTENT-DRIVEN TEMPLATE VALIDATION — Slice 1: Седмица 1 (Сесия 25, 2026-08-05): изграден нов route `/kurs/sedmica-1` — вторият representative-week архетип ("Theory and History"), валидиращ че Weekly Course Hub архитектурата (ADR-010) реално мащабира отвъд Седмица 8. Два нови reusable компонента с нула нови CSS класове (`HistoricalTimeline` reuse на `.week-timeline`; `ResearchTurnStepper` reuse на `.cbt-diagram`, адаптиран от `CbtModelDiagram`); `comparison-matrix--dual` (дефиниран, но неизползван преди тази сесия) приложен за перви път; knowledge check чрез native `<details>/<summary>`, не нов quiz engine; `DisclaimerCallout` разширен с optional `Text` параметър (backward-compatible). Source governance: `kpt_syllabus.pdf` използван само като curriculum map; 2 syllabus конфликта реконсилирани (виж `10_SESSION_LOG.md`, Сесия 25). 21 нови/актуализирани теста (общо 248/248 passing). Пълни детайли в `10_SESSION_LOG.md` (Сесия 25).

Final Pre-Flight and Foundation Commit (Сесия 24, 2026-08-05): собственикът одобри натрупаната visual/UX основа от Сесии 17–23 без допълнителни промени; изпълнен пълен pre-flight (validation + 11-route smoke test) и създаден един-единствен foundation commit (`feat: add interactive CBT learning portal foundation`, hash `115f5fa`), обхващащ всичко от предходните седем checkpoint-а. Виж `10_SESSION_LOG.md` (Сесия 24) за пълен commit hash и diffstat.

Final Layout Defect Correction (Сесия 23, 2026-08-04): собственикът извърши реален визуален преглед и докладва 12 конкретни, визуално потвърдени проблема. **Двата blocking проблема бяха реални root-cause bugs, не козметика:** (1) **хоризонтален overflow** — grid items по подразбиране имат `min-width: auto` (не `0`), затова широко/непрекъсваемо съдържание в която и да е `.learning-grid` колона можеше да разшири цялата страница вместо да задейства локалния `overflow-x: auto` на `.comparison-matrix-wrapper` — коригирано с `.learning-grid > * { min-width: 0; align-self: start; }` (втората половина на same fix едновременно реши и #2); (2) **опънати колони с празно пространство** — `align-items: start` на самия grid контейнер не е достатъчен, ако конкретен grid item няма собствен `align-self: start`; същото правило по-горе го гарантира за всеки item; (3) **дебела синя рамка около `<h1>`** — Blazor `<FocusOnNavigate Selector="h1">` реално добавя `tabindex="-1"` и фокусира h1 при всяка навигация (за screen reader announcement, не истинска клавиатурна фокусировка) — генеричното `[tabindex]:focus-visible` правило го третираше като бутон; добавено отделно, по-меко правило само за `h1/h2/h3[tabindex="-1"]`. Допълнително: nested "карта в карта" chrome премахнат (ModuleCard вече без собствена пълна рамка — divider pattern); `DisclaimerCallout` вече поддържа `Variant` ("educational" спокоен индиго по подразбиране, "safety" остава запазен за силни ограничения) — вече не изглежда като error съобщение; Модул 1/Модул 2 дясната колона преработена в реален `.module-path` (номерирани link nodes + connector линия + ясно разграничен status node) + нова concept map секция; 18 duplicate role-label/heading двойки коригирани в цялото приложение (напр. "Проверка"/"Проверка на разбирането" → "Рефлексия"/"Проверка на разбирането"); "Наличен" статус текст премахнат за наличните уроци (CTA вече го подразбира); sidebar разширен на 276px с по-плътен padding (без неудобно пренасяне на "Обучителна програма"); Home hero компресиран (по-малко top padding, по-стегнат `line-height` на h1); top bar получи малка platform икона. 42 нови теста (общо 227/227 passing). Пълни детайли в `10_SESSION_LOG.md` (Сесия 23).

## Repository — статус (2026-07-30)

| Параметър | Стойност |
|---|---|
| Repository root | project root (съдържа `00_PROJECT_OS/`, `code_artifact.html`, `CbtLearningPlatform/`) — потвърдено чрез `git rev-parse --show-toplevel` |
| Branch | `main` |
| `.gitignore` | официален `dotnet new gitignore` темплейт, `.vscode/` селективно (не изцяло игнорирана) |
| Git identity | зададена **локално** (`--local`, само това repository); global конфигурация непроменена |
| Baseline commit | **CREATED** — hash виж `10_SESSION_LOG.md` (не се записва email в Project OS) |
| Remote | не съществува (не е част от обхвата) |
| CI workflow | `.github/workflows/ci.yml` — restore + build + test, конфигуриран локално, никога не е изпълняван на GitHub (няма remote) |
| Test project | `CbtLearningPlatform.Tests` (xUnit, `net10.0`) — 4 теста, всички passing |
| Error handling | `ErrorBoundary` (Routes.razor) + българска `Error.razor` + преведен `#blazor-error-ui` банер; server-side `UseExceptionHandler` от темплейта, проверен реално в Production среда |
| Design system | `app.css` — tokens (color/typography/spacing/shape/layout) + компоненти (бутони/карти/callout/nav/форми); `DisclaimerCallout` shared component; `MainLayout`/`Home`/`NotFound` реализирани с реално съдържание |
| Public pages | `/`, `/programa`, `/kpt`, `/programa/modul-1` + 1 урок, `/programa/modul-2` + 3 урока — реални, честни, с `ModuleCard`/`LearningObjectives`/`SourceReferences` компоненти; nav active state (`NavLink`); Google Fonts CDN — временно решение (Variant B), не окончателно production |
| Learning content | Модул 1 (1 урок) + Модул 2 (3 урока, горна граница на REQ-CONT-002) — `REQUIRES PROFESSIONAL REVIEW`, не публикувани за реални потребители (RISK-010, няма щатен клиничен рецензент) |
| Content architecture | `KEEP RAZOR FOR MVP` (Content Pipeline Decision, STEP-3.3) — Markdown/JSON pipeline не се изгражда; преразглежда се при съдържание извън капацитета на флагманския Модул 2 |

## Environment — актуален статус (2026-07-30)

| Компонент | Статус |
|---|---|
| .NET SDK | `10.0.302` инсталиран и потвърден (`C:\Program Files\dotnet\sdk\10.0.302`) |
| .NET Runtimes | 6.0.35, 8.0.17 (запазени непроменени), 10.0.10 (нови) |
| VS Code / Claude Code / Git | Работят |
| Visual Studio (пълен IDE) | Все още не е инсталирана — `OPTIONAL`, не блокира |
| Blazor Web App solution | Съществува — `CbtLearningPlatform/` (2 проекта + `.sln` + `global.json`), build чист след Git init (0/0) |
| Git repository в проекта | **Съществува** (project root), 4 commits (baseline + CI workflow + test foundation + error handling), няма remote |

## Какво е проверено

- `git rev-parse --show-toplevel` → сочи към project root, не към `CbtLearningPlatform/`.
- `git check-ignore -v` → `bin/`/`obj/` пътища игнорирани; `02_CURRENT_STATUS.md`, `code_artifact.html`, `global.json`, `.sln` — НЕ игнорирани.
- `git status --short` след `git add .` → точно 48 файла (A), без `bin/`/`obj/`/`.vs/`.
- `git diff --cached --stat` → `48 files changed, 4538 insertions(+)`.
- Secret scan (`.env`, `*.pfx`/`*.p12`/`*.pem`/`*.key`, `appsettings*` съдържание) → без открити тайни.
- `dotnet restore` + `dotnet build` след Git init → 0 Warning(s), 0 Error(s) (без регресия).
- `code_artifact.html` — непроменен (122142 bytes, timestamp непроменен).

## Активни проблеми

- Visual Studio (пълен IDE) все още не е инсталирана — `RECOMMENDED`, не блокира.
- Клиничен рецензент все още липсва — не блокира техническа работа.
- ~~Anchor-navigation риск (bare `href="#id"` резолва спрямо `App.razor`-овия `<base href="/">`)~~ — **RESOLVED и committed (Сесия 31).** Виж `10_SESSION_LOG.md` (Сесии 29–31) за пълната диагноза и поправка.
- **Повтарящ се environment проблем:** `dotnet run` фонови процеси в тази Windows/Git-Bash среда понякога надживяват bash `kill` (не се виждат от `ps aux`), задържайки порт 5055 за следваща сесия/стъпка — потвърдено седем пъти (Сесия 17–23). Обратен случай, открит в Сесия 23: `nohup ... &` фонов процес умря сам (без crash в лога) между стъпки, оставяйки `http://localhost:5055` недостъпен за собственика — коригирано с `disown` след `&`, не гарантирано решение за тази среда. Друга находка (Сесия 23): `Get-NetTCPConnection -LocalPort 5055` понякога показва "призрачни" `TimeWait` записи с `OwningProcess 0` (System Idle Process) от вече затворени HTTP заявки — тези НЕ блокират нов `LISTEN`; проверявай `State` колоната, не само присъствие на запис, преди да заключиш, че портът е зает. Винаги проверявай `Get-NetTCPConnection -LocalPort 5055` + `Get-Process -Name "CbtLearningPlatform"` през PowerShell преди нов smoke test, не разчитай само на предходно "процесът спрян чисто" съобщение от bash.

## Блокиращи проблеми

Няма.

## Следваща препоръчана задача

Следваща фаза: `CONTENT-DRIVEN TEMPLATE VALIDATION — WEEK 10` — собственическо решение дали и кога да стартира, изисква ново извикване, **не е започната автоматично**. `KEEP RAZOR FOR MVP` остава в сила. Реален GitHub Actions run остава `PENDING` до създаване на remote.

## Последна актуализация

2026-08-06 — Сесия 31, Week 3 Final Pre-Flight and Commit (включващ Systemic Anchor Fix).

## Общ приблизителен прогрес

Фаза 0: 100%. Фаза 1: ~92% (5.5 от 6 STEP-а). Фаза 2: ~40% (design system + 2 публични страници). Фаза 3: ~30% (7 страници, 4 реални урока — Модул 1 + флагманският Модул 2 съдържателно завършени за MVP). Общ проект (Фази 0–9): ~34%.
