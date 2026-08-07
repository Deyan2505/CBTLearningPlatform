# 04 — Changelog

Формат: най-новото най-отгоре. Всеки запис отбелязва датата и типа промяна (добавено / променено / поправено / премахнато / архитектура / база данни / документация).

---

## 2026-08-07 — Week 10 Final Pre-Flight and Commit (Сесия 33, финал)

- **Повод:** собственикът одобри окончателно Седмица 10, включително Section 08 simplified process design.
- **Version control (добавено):** единствен commit `feat: add week 10 guided practice learning slice`, обхващащ целия Week 10 slice — изграждане (Сесия 32) + всичките корекционни pass-ове (Сесия 33: h1 фокус root-cause fix, Section 01 responsive process + arrow placement, Section 10 full-width подялба, Section 08 два-режимен simplified process + 6-`<li>` семантика) + тестове + Project OS.
- **Проверено:** restore/build (0/0), test (336/336), `git diff --check` чист, 14/14 routes `200` + български 404 на прясна инстанция, Week 10 content pre-flight (12 точки), interactive pre-flight (SocraticDialogueExplorer), Section 01/08/10 layout contracts, route-safe anchor contract (0 bare fragments, 0 дублирани id-та) — всички потвърдени. Без Co-Authored-By, без AI attribution, без push.
- **Резултат:** `WEEK 10 COMMITTED`. "Guided Practice" архетип `VALIDATED` (четвърти валидиран архетип). Следваща стъпка `OPTIONAL READING SOURCE COMPONENT` (НЕ Library phase) — не е започната автоматично.

## 2026-08-06 (продължение 11) — Section 08 Final Redesign: Simple, Clear, Intuitive Six-Step Process (Сесия 33, продължение, НЕКОМИТНАТО)

- **Повод:** собственикът отхвърли целия 3×2 layout подход, не само отделни позиции — номер/текст не групирани визуално, линии отделени от съдържанието, 03→04 неинтуитивен, 06 изолиран, много празно пространство.
- **Ново решение:** точно два responsive режима (wide хоризонтален ред / narrow вертикална колона), нищо междинно — без 3×2, без boustrophedon, без reversal. Нов `.guided-practice-sequence__unit` физически групира номер+label в едно цяло. Connector-и (5, presentation-only spans) свързват целите unit-и чрез flexbox, центрирани спрямо фиксирания размер на номера — не зависят от дължината на label текста.
- **Container query:** преизползван съществуващият `.page-container` ambient container context (без нов wrapper); breakpoint 1100px, избран емпирично според най-дългия label; `@supports not` fallback по установената конвенция.
- **Премахнат dead CSS:** целият 3×2 grid-placement блок (repeat(3), nth-child hack-ове, display:none workaround) изцяло изтрит.
- **Тестове:** 2 нови факта (unit grouping; точно два режима, без dead 3×2 contract, без CSS `order`, без absolute positioning). 336/336 общо.
- **Проверено:** build/test 0/0 → 336/336, `git diff --check` чист, fresh-server structural QA потвърждава новата семантика без регресия на Section 01/Седмица 1/3/8.
- **Резултат:** `WEEK 10 SECTION 08 SIMPLIFIED PROCESS READY — OWNER REVIEW REQUIRED`. Некомитнато.

## 2026-08-06 (продължение 10) — Week 10 Visual Corrections: Section 01 Arrow Placement + Section 08 Broken-Flow Look (Сесия 33, продължение, НЕКОМИТНАТО)

- **Повод:** нов собственически визуален преглед — Section 01's turn connector изглежда сякаш тръгва от грешна стъпка; необозначена "втора зона" все още визуално обърква, изисквана честна диагноза.
- **Issue 1 root cause:** Section 01's turn connector (стъпка 3→4) беше центриран през целия ред вместо подравнен под колоната, в която реално стоят стъпка 3 и стъпка 4 (колона 5) — визуално изглеждаше, че пада от стъпка 2.
- **Issue 1 fix:** `app.css` — connector преместен на `grid-column: 5`, директно под/над съответните стъпки. Само CSS, layout идеята запазена.
- **Issue 2 диагноза:** Section 08's стъпка 3 (нейният нов 3×2 rail) получаваше центриран marker, докато съседните ѝ стъпки (1,2,4,5) имат ляво подравнен marker — единствената несъответстваща не-финална стъпка, четяща се като прекъснат/полу-счупен rail.
- **Issue 2 fix:** `app.css` — премахнато центрирането за стъпка 3; вече ляво подравнена като съседите си, консистентен вид. Центрирането остава само за истински финалната стъпка 6.
- **Засегнати файлове:** единствено `app.css`. Без DOM/съдържание промени.
- **Тестове:** без нови (чисто CSS). 334/334 непроменено.
- **Проверено:** build/test 0/0 → 334/334, `git diff --check` чист, fresh-server QA потвърждава двете корекции без регресия на Седмица 1/3/8/anchors/accessibility (DOM недокоснат).
- **Резултат:** `WEEK 10 VISUAL CORRECTIONS APPLIED — OWNER VISUAL REVIEW REQUIRED`. Некомитнато.

## 2026-08-06 (продължение 9) — Section 08 Semantic Correction: Six Steps Must Mean Six List Items (Сесия 33, продължение, НЕКОМИТНАТО)

- **Повод:** предходният handoff призна собствен accessibility/semantic дефект — Section 08's `<ol>` съдържаше 11 `<li>` (6 markers + 5 connector-и) вместо точно 6, защото connector-ите бяха отделни sibling `<li>` елементи, участващи погрешно в list semantics ("item N of 11" вместо "item N of 6").
- **Поправено (Sedmica10.razor):** `<ol class="guided-practice-sequence">` вече съдържа точно **6 `<li>`** (един по стъпка). Connector-ите станаха `<span aria-hidden="true">`, вложени вътре в предходния `<li>`, presentation-only, без `role="listitem"`. Последната стъпка (06) няма connector.
- **Поправено (app.css, visual redesign заради двусмисленост):** предходният 4+2 boustrophedon (ред 2 обърнат) заменен с по-прост **3×2 grid** — ред 1 = 01→02→03, ред 2 = 04→05→06, и двата отляво надясно, без reversal (точно като пренасяне на текстов ред). Connector-ът за row wrap-а (03→04) е скрит — линия там би подвела; поредната номерация е достатъчна. Позициониране чрез explicit CSS Grid placement, не CSS `order`.
- **Тестове:** 3 нови факта — точно 6 `<li>`, connector-ите не са собствени `<li>`, 5 presentation-only connector spans, без connector след финалната стъпка, `--final` модификаторът остава. 334/334 общо.
- **Проверено:** build/test 0/0 → 334/334, `git diff --check` чист, fresh-server structural QA потвърждава точната 6-`<li>` семантика и без регресия на Section 01/Седмица 1/3/8.
- **Резултат:** `SECTION 08 SEMANTICS FIXED — OWNER VISUAL REVIEW REQUIRED`. Некомитнато.

## 2026-08-06 (продължение 8) — WEEK 10 Redesign Section 08 Only: Practical Sequence as a Real Process Visual (Сесия 33, продължение, НЕКОМИТНАТО)

- **Повод:** визуален преглед на трите Сесия-33 корекции отхвърли Section 08 — пълноширочинна секция, но шестте стъпки все още подредени като тесен вертикален stack, огромно неизползвано пространство отдясно, недостатъчна визуална стойност за реален process model.
- **Поправено (Sedmica10.razor + app.css):** нов, изрично scoped `.guided-practice-sequence` pattern — номерирани кръгли markers на свързваща хоризонтална "релса", **не** reuse на `.concept-map__flow`/`__node`/`__connector`. Mobile: вертикална релса. Desktop (900px+): 4+2 converging layout (ред 1 = стъпки 1-4 през пълната ширина; централен turn marker; ред 2 = стъпки 5-6 центрирани, 05 дясно/06 ляво-highlighted) чрез explicit `nth-child` grid placement. DOM/reading order 1→6 запазен.
- **Съдържание:** шестте стъпки и educational note-ът запазени дословно; добавени само позиционни номера 01-06 в markers-ите (съответстват на собственическия mockup).
- **Тестове:** 3 нови факта в `Week10ContentSliceTests.cs` (scoped pattern, DOM ред/съдържание дословно запазени, изолация от другите седмици и от Section 01). 331/331 общо.
- **Проверено:** build/test 0/0 → 331/331 (стар сървърен процес, заключил build изхода, идентифициран и спрян преди build), `git diff --check` чист, fresh-server structural QA (decoded HTML, коригирана slice граница до следващата секция) потвърждава точна DOM структура (11 `<li>`: 6 markers + 5 connectors), без регресия на Section 01/Седмица 1/3/8/route-safe anchors.
- **Резултат:** `WEEK 10 SECTION 08 PROCESS RAIL READY — OWNER APPROVAL REQUIRED`. Некомитнато.

## 2026-08-06 (продължение 7) — WEEK 10 Owner Visual Review: Final Layout Corrections (Сесия 33, НЕКОМИТНАТО)

- **Повод:** собственическият визуален преглед на некомитнатата Седмица 10 одобри архетипа като цяло и поиска 3 точкови корекции: `<h1>` фокус рамка, тесен Section 01 process layout на desktop, празна дясна половина на Section 10.
- **Поправено (`<h1>` фокус рамка, глобален CSS):** Сесия-23-ото предположение за `tabindex="-1"` и реална клавиатурна фокусировка е фактически невярно — коригирано разбиране; `:focus`/`:focus-visible` слети в едно безусловно `outline: none` правило за `h1/h2/h3[tabindex="-1"]`; ретроактивно потвърдено и за Седмица 1/3/8 без техни промени.
- **Поправено (Section 01, Sedmica10.razor + app.css):** нов scoped `.concept-map__flow--process` модификатор (`@media (min-width: 900px)`) — boustrophedon 6-стъпков layout чрез explicit `nth-child` grid placement + ротирани "↓" стрелки; DOM/visual order 1→6 запазен; всяка друга `.concept-map__flow` употреба недокосната.
- **Поправено (Section 10, Sedmica10.razor):** премахнат single-child `.learning-grid--balanced` wrapper (същият defect клас, недокоснат в Седмица 3); нов вътрешен пълноширочинен `.learning-grid--balanced` с 4 sibling блока, разчитащ на естественото CSS Grid row-major auto-placement — 0 нов CSS, без `order`.
- **Тестове:** 7 нови факта в `Week10ContentSliceTests.cs`; 1 тест в `LayoutDefectFixTests.cs` преименуван/коригиран за верния `tabindex="-1"` разбор. 328/328 общо.
- **Проверено:** build/test 0/0 → 328/328, `git diff --check` чист, fresh-server structural QA (curl + decoded HTML) потвърждава всички 3 корекции и липсата на регресия на Седмица 1/3/8. Headless browser недостъпен в тази среда — pixel-level breakpoint QA не бе технически възможна, отбелязано изрично.
- **Резултат:** `WEEK 10 FINAL LAYOUT CORRECTIONS READY — OWNER APPROVAL REQUIRED`. Некомитнато — изчаква собственическо одобрение.

## 2026-08-06 (продължение 6) — CONTENT-DRIVEN TEMPLATE VALIDATION, Slice 3: Седмица 10 (Сесия 32, НЕКОМИТНАТО)

- **Повод:** върху committed-ата Седмица 3, собственикът стартира третия content-driven vertical slice — само Седмица 10, без Седмица 2/4/9/11, без глобален redesign, без промяна на вече валидираните архетипи.
- **Съдържание (добавено):** нов route `/kurs/sedmica-10` — четвъртият representative-week архетип на Weekly Course Hub-а, "Guided Practice" (сократически въпроси, четири семейства въпроси, guided dialogue explorer, факт/предположение/заключение, декатастрофизиране, балансиран отговор).
- **Компоненти (добавени, 0 нови CSS класа):** `SocraticDialogueExplorer.razor` (wholesale reuse на `.cbt-diagram`).
- **Source reconciliation (реконсилирани 4+ конфликта):** сократическите въпроси не са разпит/прикрит съвет/риторика/доказателство, че мисълта е грешна; балансираният отговор не е принудителен позитивизъм; декатастрофизирането не омаловажава реален проблем; алтернативата не отменя първото обяснение само защото е по-приятна.
- **Тестове:** нов `Week10ContentSliceTests.cs` (24 факта); `CurriculumHubTests.cs`/`Week1ContentSliceTests.cs`/`Week3ContentSliceTests.cs`/`ContentSliceTests.cs` актуализирани за четвъртата налична седмица. 321/321 общо.
- **Проверено:** build 0/0, `git diff --check` чист, 14/14 routes 200 на прясна инстанция, route-safe anchors от самото начало, decoded HTML потвърждава 0 изтекли вътрешни термини, 0 диагностични скали, 0 personal-data inputs.
- **Резултат:** `WEEK 10 GUIDED-PRACTICE VERTICAL SLICE READY — OWNER CONTENT AND VISUAL REVIEW REQUIRED`. Некомитнато — изчаква собственически преглед.

## 2026-08-06 (продължение 5) — Week 3 Final Pre-Flight and Commit, including Systemic Anchor Fix (Сесия 31)

- **Повод:** собственикът одобри Седмица 3, anchor навигацията на Седмици 1/3/8, глобалния skip-link и поправения section flow на Седмица 3.
- **Version control (добавено):** единствен commit `feat: add week 3 cognitive model slice and route-safe anchors`, обхващащ целия Content-Driven Vertical Slice — Week 3 (нов route, 2 нови interactive компонента) плюс целия systemic route-safe anchor fix (Седмица 1/3/8 + global skip-link).
- **Проверено:** restore/build (0/0), test (296/296), 13/13 routes 200 на прясна инстанция, anchor navigation pre-flight (Седмица 1/3/8 + skip-link на 6 routes), Week 3 content pre-flight (14 точки), Week 3 layout pre-flight (без negative margins/absolute positioning/fixed heights) — всички потвърдени.
- **Резултат:** `WEEK 3 AND SYSTEMIC ANCHOR FIX COMMITTED`. "Concept and Diagram" архетип `VALIDATED`. Systemic route-safe anchor contract `COMPLETE`. Следваща фаза `CONTENT-DRIVEN TEMPLATE VALIDATION — WEEK 10` — не е започната автоматично.

## 2026-08-06 (продължение 4) — Systemic Route-Safe Anchor Fix (Сесия 30, НЕКОМИТНАТО)

- **Повод:** Сесия 29 установи, че anchor-navigation бъгът (bare `href="#id"` резолва спрямо `App.razor`-овия `<base href="/">`, не спрямо текущата страница) е системен, потвърден и в вече committed-натите `Sedmica1.razor`/`Sedmica8.razor` и в site-wide skip-link-а. Тази сесия систематично поправя всички потвърдени случаи.
- **Поправено:** `Sedmica1.razor` (9 section-nav anchors), `Sedmica8.razor` (8 section-nav anchors) вече route-safe (`href="/kurs/sedmica-{N}#id"`) — идентично на вече прилoжената поправка в `Sedmica3.razor`. Site-wide skip-link в `MainLayout.razor` вече изчислява целта динамично чрез `NavigationManager` (`{uri.AbsolutePath}{uri.Query}#main-content`), тъй като целевата страница се променя на всеки route — не може да е статичен път.
- **Недокоснато (умишлено):** `<base href="/">` в `App.razor` — остава непроменен, задължителен за Blazor asset/SignalR resolution.
- **Тестове:** нов `SystemicAnchorFixTests.cs` — включва whole-`Components/`-tree regression тест (`NoBareFragmentAnchorsRemainAnywhereInComponents`), потвърждаващ, че никой `.razor` файл вече не съдържа bare fragment href. Остарял `Week8Page_SectionNavigatorLinksToAllEightAnchors` (`SectionArchitectureTests.cs`) актуализиран за route-safe hrefs. 9 нови/актуализирани теста общо (296/296 passing).
- **Проверено:** build 0/0, `git diff --check` чист, fresh-server smoke test потвърждава skip-link реално сервира различен href на всяка тествана страница, 0 bare fragments в served HTML на трите седмици.
- **Резултат:** anchor-navigation рискът е технически разрешен навсякъде, където беше потвърден. Некомитнато — засяга и вече committed файлове, чака собственическо одобрение.

## 2026-08-06 (продължение 3) — WEEK 3 Owner Review: Anchor Navigation and Grid Gap Fix (Сесия 29, НЕКОМИТНАТО)

- **Повод:** собственически преглед на живо на `/kurs/sedmica-3` докладва 2 blocking дефекта — вътрешна anchor навигация отваря Home вместо да скролва на място; голяма празна зона под "Чести обърквания" поради height-mismatch с "Проверка на разбирането" в общ grid ред.
- **Поправено (root cause #1):** `App.razor`-овият `<base href="/">` резолва bare `href="#id"` спрямо "/", не спрямо текущата страница (HTML spec поведение, не Blazor-специфично) — section-nav hrefs в `Sedmica3.razor` вече включват пълния път (`/kurs/sedmica-3#id`).
- **Докладвано, не поправено:** идентичен риск в вече committed-натите `Sedmica1.razor`/`Sedmica8.razor` и в site-wide skip-link-а в `MainLayout.razor` — извън обхвата на тази стъпка, изисква отделно собственическо решение.
- **Поправено (root cause #2):** секции "Чести обърквания"/"Проверка на разбирането" разделени в собствени пълноширочинни `LearningSection` блокове вместо споделен двуколонен ред; 3-те карти на "Чести обърквания" минаха от fixed 2-col grid (оставящ orphan) на auto-fit `.concept-map__side-notes`.
- **Тестове:** 2 нови regression теста (hrefs включват пълния път; секциите не споделят grid wrapper). 287/287 общо.
- **Резултат:** двата blocking дефекта коригирани в Седмица 3; известен, документиран риск за Седмица 1/8/skip-link остава открит за собственическо решение.

## 2026-08-06 (продължение 2) — CONTENT-DRIVEN TEMPLATE VALIDATION, Slice 2: Седмица 3 (Сесия 28, НЕКОМИТНАТО)

- **Повод:** върху committed-ата Седмица 1, собственикът стартира втория content-driven vertical slice — само Седмица 3, без Седмица 2/4/10, без глобален redesign, без commit в тази стъпка.
- **Съдържание (добавено):** нов route `/kurs/sedmica-3` — третият representative-week архетип на Weekly Course Hub-а, "Concept and Diagram" (йерархия автоматична мисъл→междинно вярване→основно вярване, когнитивна триада, schema-as-filter демонстрация, автоматична/рефлексивна обработка, интегрирана concept map).
- **Компоненти (добавени):** `CognitiveHierarchyExplorer.razor` (reuse на `.cbt-diagram` wholesale, 0 нови CSS класа); `SchemaFilterDemonstration.razor` (нов малък, scoped `.schema-filter` CSS блок за toggle+list UI, реален необходим нов компонент, не generic framework).
- **Source reconciliation (4 конфликта, документирани):** основните вярвания не са задължително отрицателни/патологични; "схема като филтър" изрично обозначена като учебна метафора, не буквален механизъм; "автоматично" не се приравнява на "ирационално"; рефлексивната обработка изрично не гарантира безгрешен резултат.
- **Тестове:** нов `Week3ContentSliceTests.cs` (26 факта); `CurriculumHubTests.cs`/`Week1ContentSliceTests.cs`/`ContentSliceTests.cs` актуализирани за третата налична седмица. 285/285 общо.
- **Проверено:** build 0/0, `git diff --check` чист, 13/13 routes 200 на прясна инстанция, decoded HTML потвърждава 0 изтекли вътрешни термини, 0 диагностични скали, 0 personal-data inputs.
- **Резултат:** `WEEK 3 CONCEPT-AND-DIAGRAM VERTICAL SLICE READY — OWNER CONTENT AND VISUAL REVIEW REQUIRED`. Некомитнато — изчаква собственически преглед.

## 2026-08-06 (продължение) — Week 1 Final Pre-Flight and Commit (Сесия 27)

- **Повод:** собственикът одобри съдържанието и визуалната реализация на Седмица 1 (Сесии 25–26 резултат).
- **Version control (добавено):** единствен commit `feat: add week 1 CBT history learning slice`, обхващащ целия Content-Driven Vertical Slice — Week 1 (нов route, 2 нови компонента, sidebar contextual state, всички content-driven корекции от Сесия 26).
- **Проверено:** restore/build (0/0), test (258/258), 12/12 routes 200 на прясна инстанция, friendly 404, dark/light theme, content pre-flight (title/badge/reconciled формулировки/1979 цитат/learner-facing academic context/български review статус/educational disclaimer) — всички потвърдени.
- **Резултат:** `WEEK 1 CONTENT-DRIVEN VERTICAL SLICE COMMITTED`. "Theory and History" архетип `VALIDATED`. Следваща фаза `CONTENT-DRIVEN TEMPLATE VALIDATION — WEEK 3` — не е започната автоматично.

## 2026-08-06 — CONTENT-DRIVEN TEMPLATE VALIDATION, Owner Review Correction (Сесия 26, НЕКОМИТНАТО)

- **Повод:** собственически визуален преглед на Седмица 1 (`/kurs/sedmica-1`) — архитектурата потвърдена, поискани само точкови content-driven корекции.
- **Поправено (публичен HTML):** премахнати всички вътрешни development термини (файлови имена, `citation-grade`, uppercase `ACADEMIC/CLINICAL REVIEW PENDING`) от рендерираното съдържание — заменени с learner-facing академичен текст и обикновено българско review изречение.
- **Поправено (layout):** нов scoped `.research-turn-stepper` CSS Grid variant за 4-те стъпки на `ResearchTurnStepper` (равноправни tracks на desktop, zigzag на средна ширина, вертикално на mobile, без dangling arrow, без absolute positioning) — споделеният `CbtModelDiagram`/`.cbt-diagram` layout остава непроменен.
- **Добавено:** `HistoricalTimeline.Compact` density параметър + `.week-timeline--compact` CSS (Course Hub-овата собствена timeline непроменена); "weak parent context" sidebar слой в `MainLayout` (`/kurs/*`, `/programa/*`) — отделен от NavLink-овия силен exact-match `.active`.
- **Поправено (съдържание):** скъсено Week 1 заглавие; content-format badge "Теория и история" вместо availability pill; преформулирана non-scored knowledge-check инструкция; Section 09 разбита на 3 подблока (Какво да запомните / Академичен контекст / Източници и следващи стъпки); Section 01 сведена до основната си теза.
- **Ключов резултат (документиран изрично):** споделените timeline/diagram patterns бяха преизползваеми принципно, но реалното историческо съдържание изискваше compact density и отделен responsive stepper layout — валиден резултат от content-driven валидацията, не грешка в дизайна.
- **Тестове:** 10 нови факта в `Week1ContentSliceTests.cs`. 258/258 общо.
- **Проверено:** build 0/0, `git diff --check` чист, 12/12 routes 200, decoded HTML потвърждава 0 изтекли вътрешни термини, sidebar active/context комбинации коректни на всички проверени routes.
- **Резултат:** `WEEK 1 CONTENT AND TEMPLATE CORRECTIONS READY — OWNER APPROVAL REQUIRED`. Некомитнато.

## 2026-08-05 (продължение) — CONTENT-DRIVEN TEMPLATE VALIDATION, Slice 1: Седмица 1 (Сесия 25, НЕКОМИТНАТО)

- **Повод:** върху одобрения и committed-ан foundation (`115f5fa`), собственикът стартира новата фаза с изричен, тесен обхват — само Седмица 1, без redesign, без промяна на application shell, без commit в тази стъпка.
- **Съдържание (добавено):** нов route `/kurs/sedmica-1` — вторият representative-week архетип на Weekly Course Hub-а, "Theory and History" (историческа времева линия, causal research-turn interactivity, before/after сравнение на изследователския фокус, 1979 milestone, non-scored knowledge check), различен от Седмица 8-ия "Simulator Workspace" архетип.
- **Компоненти (добавени, нула нови CSS класове):** `HistoricalTimeline.razor` (Components/Shared, static SSR — reuse на `.week-timeline`); `ResearchTurnStepper.razor` (Client/Interactive, единствен interactive island — reuse на `.cbt-diagram`, адаптиран директно от `CbtModelDiagram`). `comparison-matrix--dual` (дефиниран по-рано, неизползван) приложен за първи път.
- **Компоненти (разширени):** `DisclaimerCallout` получи optional `Text` параметър (backward-compatible — default текст непроменен, съществуващите извиквания без параметър работят идентично).
- **Source governance:** `kpt_syllabus.pdf` използван само като curriculum map, не като цитируем източник; институционално оформление изрично изключено. Основен източник — SRC-041. Двете дати (1964 контекстуално потвърдена, 1979 показана директно) проверени срещу `10_SESSION_LOG.md` GAP-012 (Closed).
- **Source reconciliation (2 конфликта, документирани, не мълчаливо решени):** автоматичните мисли не са представени като "лишени от обективна валидност" (заменено с вече заключената формулировка); психоанализата не е представена като "провалена" — преходът е описан като промяна на изследователския въпрос, не като "победа" на школа.
- **Тестове:** нов `Week1ContentSliceTests.cs` (19 теста); `CurriculumHubTests.cs`/`ContentSliceTests.cs` актуализирани за втората налична седмица. 248/248 общо (227 baseline + 21 нови/актуализирани).
- **Проверено:** build 0/0, `git diff --check` чист, smoke test на всичките 12 реални routes на прясна инстанция (всички `200`), 0 ECTS/институционален бранд/диагностично съдържание, exactly 1 interactive island, 0 нови CSS класа.
- **Резултат:** `WEEK 1 CONTENT-DRIVEN VERTICAL SLICE READY — OWNER CONTENT AND VISUAL REVIEW REQUIRED`. Некомитнато — изчаква собственически преглед. Седмица 2/3/10 не са започнати.

## 2026-08-05 — Final Pre-Flight and Foundation Commit (Сесия 24)

- **Повод:** собственикът одобри натрупаната visual/UX основа от Сесии 17–23 без допълнителни промени; поискан е само финален pre-flight и един общ commit.
- **Version control (добавено):** единствен foundation commit `feat: add interactive CBT learning portal foundation`, обхващащ всичко натрупано некомитнато от Сесии 17–23 (Weekly Course Hub, Седмица 8, dark-first design system, три интерактивни компонента, глобален двуколонен redesign, всички layout defect corrections), плюс собственически одобрения `kpt_syllabus.pdf`.
- **Документация (поправено):** `02_CURRENT_STATUS.md` — премахнати остатъчни, противоречиви записи от Сесия 16 ("Следваща препоръчана задача"/"Последна актуализация"); добавено изрично шестточково разграничение на обхвата (visual foundation / Weekly Hub foundation / Седмица 8 / останалите 14 седмици / клиничен review / следваща фаза).
- **Проверено:** `dotnet restore`/`build` (0/0), `dotnet test` (227/227), 11-route smoke test на прясна инстанция (всички `200`, dark default, light достъпна, без ECTS/диагностично съдържание), без `overflow-x: hidden` в CSS.
- **Резултат:** `FOUNDATION COMMIT COMPLETE`. Следваща фаза `CONTENT-DRIVEN TEMPLATE VALIDATION` — не е започната автоматично.

## 2026-08-04 (продължение 2) — Final Layout Defect Correction (НЕКОМИТНАТО, върху Сесия 17–22)

- **Повод:** реален собственически screenshot преглед — 12 конкретни визуално потвърдени проблема (overflow, focus ring, stretched panels, nested chrome, слаб learning path, error-styled disclaimer, duplicate labels, слаб status text, sidebar wrapping, hero density, празен top bar).
- **Root-cause fix (blocking #1+#2):** `.learning-grid > * { min-width: 0; align-self: start; }` — grid items по подразбиране имат `min-width: auto` (форсира page-level overflow вместо локален `overflow-x: auto` в `.comparison-matrix-wrapper`) и `align-self: stretch` не се override-ва автоматично само от `align-items: start` на контейнера за всеки item. Едно правило коригира и двата blocking проблема едновременно.
- **Root-cause fix (blocking #3):** Blazor `<FocusOnNavigate Selector="h1">` реално добавя `tabindex="-1"` + `.focus()` на всяка навигация — генеричното `[tabindex]:focus-visible` правило рендираше дебела бутон-стил рамка около обикновен текст. Ново, по-меко правило само за `h1/h2/h3[tabindex="-1"]` (без outline на обикновен `:focus`, мек outline само на реален `:focus-visible`).
- **Nested card chrome:** `ModuleCard.razor` вече без собствена `.card` рамка (винаги е вътре в вече рамкиран `LearningSection`) — `.module-list` вече е single-surface с divider-и между записите.
- **DisclaimerCallout:** нов `Variant` параметър — `"educational"` (спокоен индиго, по подразбиране) вместо `"safety"` (rose, вече запазен само за силни ограничения, не default). `.callout--educational` е и визуално по-лек (само left-accent, без пълна рамка) — не второ вложено warning-табло.
- **Module 1/2 learning path:** нов `.module-path` (номерирани link nodes + connector линия + ясно разграничен, не-кликаем status node), заменя плоския `concept-map__flow` списък от заглавия. Добавена и отделна concept-map секция за всеки модул.
- **Duplicate labels:** 18 role-badge/heading двойки коригирани в цялото приложение — "Уроци"→"Модул 1/2", "Последователност"→"Учебен път", "Проверка"→"Рефлексия", "Обобщение"→"Изводи", "Следваща стъпка" badge→"Напред", "Как се използва"→"Насоки", "Видове съдържание"→"Формати".
- **"Наличен" cleanup:** `ModuleCard` вече показва status текст само в disabled (still-unavailable) случая — наличните уроци го подразбират чрез самия активен CTA бутон.
- **Sidebar:** ширина 240px→276px, по-плътен padding/icon gap — "Обучителна програма" вече не се пречупва неудобно.
- **Home hero:** намален top padding, по-стегнат `h1 line-height: 1.15`, по-малка междина до първия двуколонен ред — по-голяма част от Home се вижда в първия viewport на 1920×1080.
- **Top bar:** малка platform SVG икона + `align-items: center`, по-компактен вертикален padding, по-компактен theme toggle.
- **Тестове (добавено):** `LayoutDefectFixTests.cs` (нов, 42 теста). Общо 227/227 passing.
- **Проверено:** build 0/0; сървърен процес спрян по PID (плюс откриване и документиране на "phantom TimeWait" фалшиво-положителен резултат при проверка на порта); рестартиран с новия build; реален HTTP smoke test — всичко потвърдено реално сервирано, сървърен лог чист.
- **Статус:** `FINAL LAYOUT DEFECTS FIXED — OWNER APPROVAL REQUIRED`. **Промените НЕ са commit-нати** — седми натрупан checkpoint върху некомитнатата Сесия 17–22 работа.

## 2026-08-04 (продължение) — Final Visual Polish (НЕКОМИТНАТО, върху Сесия 17–21)

- **Повод:** собственикът потвърди, че глобалната двуколонна архитектура е правилната посока (центрирано, двуколонно, цветово различимо) и поиска само финален polish — не нов redesign.
- **Код (реален bug fix):** sidebar `NavLink` елементите разчитаха на подразбиращия се `NavLinkMatch.Prefix`, което правеше родителя и текущата цел активни едновременно на вложени routes (напр. `/programa` + `/programa/modul-1` едновременно "active" на `/programa/modul-1`). Добавен изричен `Match="NavLinkMatch.All"` на всичките 6 sidebar линка — потвърдено с реален HTTP тест: точно 1 active клас на модул route, 0 на lesson route (без изкуствено parent bleed-through).
- **Код (Home):** вторичното CTA "Какво е КПТ" минава от самостоятелен текстов линк на реален `.btn-blue` outlined бутон (background/border/hover/icon, същата 44px височина като primary). "Научи → Разгледай → Приложи" пренаписан от три текстови кутии в `.learning-path-diagram` — номерирани, ролево оцветени (blue/violet/teal) стъпки с inline SVG икони и directional connector стрелки (хоризонтално на desktop, вертикално на mobile), semantic `<ol>` fallback.
- **Код (4-те реални урока):** теоретичната секция на всеки урок вече показва първо кратка дефиниция + 3 ключови извода (`.key-idea-strip`) + мини пример, с "Покажи пълното обяснение" (`ProgressiveExplanation`) за разширения текст/граници/връзка с модела. Учебни цели, дефиниция, visual anchor и disclaimer остават винаги видими — тествано автоматично (disclaimer никога вътре в disclosure блок).
- **Visual anchors (подобрени по урок):** Модул 1 Урок 1 — икони в сравнителната "образование срещу терапия" таблица, две ясно оцветени колони (нито една в warning/error цвят); Модул 2 Урок 1 — `CbtModelDiagram` вече показва свързан процес с directional connectors между стъпките; Урок 2 — ново трикатегорийно "факт / автоматична мисъл / чувство" сравнение (`.category-compare`, 3 цветово различими карти) вместо обикновен `<ul>`, среден възел в process диаграмата визуално подчертан; Урок 3 — формулировка "Една възможна телесна реакция" (не универсална връзка) + изрична граница в пълното обяснение.
- **Дизайн (workspace gutters):** `.page-container` padding минава от фиксиран `--space-4` на `padding-inline: clamp(24px, 2.5vw, 48px)` — дясната колона вече не докосва ръба на широк desktop viewport.
- **Дизайн (по-хладна тъмна палитра, WCAG реизчислена):** `--color-background` от топло кафяво `#1E1D1B` на navy-charcoal `#0E1420` (и surface/border/text токените съответно) — по-близо до "тъмна учебна лаборатория", по-малко кафяво, все така не чист черен, без neon. Всички accent роли (violet/theory/academic/interactive/example/safety) реално преизчислени срещу новия фон.
- **Top bar:** добавен subtitle "Основи на КПТ" до статус текста.
- **Тестове (добавено):** `FinalPolishTests.cs` (нов, 22 теста — active-nav matching count, Home CTA/learning-path структура, progressive disclosure per lesson с main-definition-outside-disclosure проверка, disclaimer-not-nested per lesson, visual anchor маркери, gutter CSS, dark palette стойност). Общо 185/185 passing (беше 159).
- **Проверено:** build 0/0; стар сървърен процес спрян точково по PID, рестартиран с новия build; реален HTTP smoke test — active nav потвърден точно 1 на модул route/0 на lesson route, Home CTA/learning-path реално сервирани, progressive-explanation маркери на всичките 4 урока, category-compare 3-те категории, "Една възможна телесна реакция" фраза, нов dark background hex реално сервиран, gutter clamp реално сервиран, h1=1/disclaimer=1 навсякъде, сървърен лог чист.
- **Статус:** `FINAL VISUAL POLISH READY — OWNER APPROVAL REQUIRED`. **Промените НЕ са commit-нати** — шести натрупан checkpoint върху некомитнатата Сесия 17–21 работа.

## 2026-08-04 — Global Two-Column Redesign (НЕКОМИТНАТО, върху Сесия 17+18+19+20)

- **Повод:** четвърти собственически визуален отказ, с реален screenshot. Двуколонният редизайн от Сесия 20 беше приложен само локално (Седмица 8 + Kurs); Home/Programa/Kpt/двата модула/четирите урока останаха в стария едноколонен `.content` шаблон. Screenshot допълнително разкри реален CSS bug: съдържанието седеше залепено вляво с огромна празна дясна зона.
- **Root-cause bug (открит и коригиран):** `.content` имаше `max-width`, но никога `margin: auto` — след като `--container-max` беше вдигнат на 1400px (Сесия 20), тесният `.content` блок вече седеше видимо накриво вляво вместо центриран. Коригирано с `margin-inline: auto`.
- **Layout (преработено):** `.learning-grid` breakpoint-ът минава от плосък viewport `@media (max-width: 900px)` към `container-type: inline-size` на `.page-container` + `@container (min-width: 760px)` — sidebar-ът изяжда реална ширина, която обикновен viewport breakpoint не вижда; добавен `@supports not (container-type: inline-size)` fallback за по-стари браузъри.
- **Код (ново):** `LearningSection.razor` (host, static SSR) — reusable content-role wrapper (border+badge), заменя "всичко е еднаква `.card`" критиката; използван 40+ пъти във всички redesign-нати страници.
- **Бутонна система (разширена):** нови `.btn-blue` (learning/details role) и `.btn-neutral` (reset/back/cancel, без accent цвят) — вече 5 отделни роли (violet/teal/blue/amber/neutral). Reset бутоните в `CbtChainSimulator`/`CategorizationCheck` минаха от teal outline на `.btn-neutral`; category-reveal бутоните в `CategorizationCheck` минаха на `.btn-example` (amber).
- **Глобално приложено (всичките 11 routes, не само 2):**
  - `Home.razor` — hero (wider `.hero-intro`, не тесен `.content`) + 3 двуколонни реда (процес стъпки+визуален модел; какво ще научите+начало; граница+защо платформата).
  - `Programa.razor` — wide-narrow (learning path + sidebar с легенди и disclaimer).
  - `Kpt.razor` — 3 реда (теория+диаграма; 2 мини сравнения; интерактивен пример+следваща стъпка).
  - `Modul1.razor`/`Modul2.razor` — wide-narrow module overview шаблон (уроци + sequence sidebar с disclaimer).
  - 4-те реални урока — всеки с нов, конкретен visual anchor: Модул 1 Урок 1 → сравнителна таблица "образование срещу терапия" (преформулировка на вече одобрения disclaimer, не ново клинично твърдение); Модул 2 Урок 2 → малка "как възниква автоматична мисъл" процес диаграма; Модул 2 Урок 3 → emotion↔body диаграма + comparison matrix + cross-link към Седмица 8 симулатора (без дублиране на симулатор на всеки урок).
- **Sidebar/top bar:** inline SVG икони (без emoji) на всичките 6 nav линка; по-силен active state (violet border+surface, вече консистентен с "violet = основен маршрут" ролята); top bar получи "Образователна платформа" статус текст.
- **Тестове (добавено):** `GlobalRedesignTests.cs` (нов, route inventory + all-routes learning-grid/visual-anchor/disclaimer checks) + разширения. Общо 159/159 passing (беше 121).
- **Проверено:** build 0/0; стар сървърен процес спрян точково по PID, рестартиран с новия build; реален HTTP smoke test на всичките 11 routes — всички 200, реални `.learning-grid` редове преброени на всяка страница (1–3 всяка), `.content` centering фикс потвърден в served CSS, container-query CSS потвърдена, 6 sidebar икони + 2 topbar статус occurrences, 5-те бутонни роли реално сервирани, h1=1/disclaimer=1 навсякъде, без ECTS/forbidden distortion string, сървърен лог чист.
- **Статус:** `GLOBAL TWO-COLUMN LEARNING WORKSPACE READY — OWNER VISUAL REVIEW REQUIRED`. **Промените НЕ са commit-нати** — пети натрупан checkpoint върху некомитнатата Сесия 17+18+19+20 работа.

## 2026-08-03 (продължение) — Two-Column Workspace + Semantic Color Refinement (НЕКОМИТНАТО, върху Сесия 17+18+19)

- **Повод:** трети собственически визуален отказ. Сесия 19 резултатът е "значително по-добър", но: работната зона зад sidebar-а остава по същество една вертикална колона; desktop ширината не се използва реално; липсва двуколонно "учебно работно пространство" усещане (LogiCraft референция); бутоните/секциите/интерактивните области използват прекалено еднообразна цветова система; различните типове съдържание не се разпознават достатъчно бързо визуално.
- **Layout (ново):** reusable `.learning-grid` система — `--balanced` (1:1), `--content-visual` (0.9:1.1), `--controls-output` (0.8:1.2), `--wide-narrow` (1.25:0.75); single-column breakpoint под 900px; DOM ред идентичен на reading order навсякъде (без CSS `order`, без screen-reader объркване). `--container-max` увеличен от 1120px на 1400px (`--workspace-max-width`/`--reading-max-width`/`--visual-max-width` именувани alias-и) — работната зона зад sidebar-а вече реално използва desktop ширината.
- **Цветова система (ново):** 6 семантични роли — `--accent-primary` (виолетов, ново, реално WCAG-изчислен: 6.00:1 dark/6.42:1 light), `--accent-theory` (синьо, ново, 7.66:1/6.18:1), `--accent-academic` (индиго, ново, 7.54:1/6.94:1), `--accent-interactive`/`--accent-example`/`--accent-safety` (alias-нати към вече верифицираните teal/amber/rose tokens — не нови изчисления, ponytail reuse). Всяка роля има text/surface/border вариант за двете теми.
- **Код (пренаписано):** `Sedmica8.razor` — 5 двуколонни реда (01 Накратко[theory]+02 Карта[visual]; 03 Симулатор с вътрешен controls-срещу-output split; 04 Сравнение[example]+05 Мисъл или емоция[example]; 06 Пълно обяснение[theory]+Академичен контекст[academic] като общ ред; 07 Проверка[theory]+08 Обобщение/disclaimer[safety]) — всяка секция с `.section-card` left-accent border + `.section-role-label` badge (без emoji).
- **Код (пренаписано):** `CbtChainSimulator.razor` — control panel и live output вече в `.learning-grid--controls-output` (controls преди output в DOM, видими едновременно на desktop).
- **Код (пренаписано):** `Kurs.razor` — `.learning-grid--wide-narrow` (timeline ляво + `.hub-sidebar` дясно: start panel/статус легенда/типове интерактивност/"как се използва курсът"/disclaimer). Съзнателно НЕ sticky — избегнат риск от footer overlap/zoom проблеми за скромна визуална полза.
- **Бутонна йерархия:** нов `.btn-violet` (виолетов, "начало"/primary action — Kurs start panel CTA), отделен от съществуващия teal `.btn-primary`/`.btn-secondary` (сега концептуално "interactive" ролята). `DisclaimerCallout.razor` преоцветен глобално от `callout--info` (teal) на `callout--safety` (rose) — консистентно навсякъде, не само на Седмица 8.
- **UI UX Pro Max:** targeted заявки за two-column workspace/split layout/semantic color — ux резултатите бяха общи layout best practices (вече приложени: reserve space, stacking context, viewport units), color резултатите бяха light-background/kids/music learning палитри — нито една директно приложима; използвани само за общ принцип (ограничен брой hues, WCAG-проверени), не буквално копирани стойности.
- **Тестове (добавено):** `LayoutRefinementTests.cs` (нов, 13 теста — grid tokens, breakpoint съдържание, 6 accent роли в двете теми, button variants, DisclaimerCallout safety role, Седмица 8 grid usage ≥4, Пълно обяснение+Академичен контекст в общ ред, section-card роли, simulator controls-преди-output ред, Kurs timeline+sidebar split, disclaimer видим в sidebar, comparison-matrix overflow guard). Общо 120/120 passing.
- **Проверено:** build 0/0; стар сървърен процес на порт 5055 идентифициран по PID и спрян точково, рестартиран с новия build; реален HTTP smoke test — 5 `.learning-grid` реда на Седмица 8, 8 section-role marker-и (3 theory/1 visual/2 example/1 academic/1 safety), controls-output grid в симулатора, timeline+hub-sidebar на `/kurs`, всичките 6 accent tokens реално сервирани в CSS, `.btn-violet` дефиниран и използван, dark/light theme запазени, без ECTS/forbidden distortion string, без fake progress "%", сървърен лог чист.
- **Статус:** `TWO-COLUMN COLOR-RICH WORKSPACE READY — OWNER VISUAL REVIEW REQUIRED`. **Промените НЕ са commit-нати** — четвърти натрупан checkpoint върху некомитнатата Сесия 17+18+19 работа.

## 2026-08-03 — Content-Rich Simulator Redesign (НЕКОМИТНАТО, върху Сесия 17+18)

- **Повод:** собственикът извърши визуален преглед на Сесия 18 резултата и **не го одобри** — оценен като "беден като информация, прекалено текстов, недостатъчно разделен на секции, без разпознаваем симулатор, по-близък до страница с карти и бутони, отколкото до интерактивен образователен портал".
- **Визуален одит (преди промени):** `/kurs/sedmica-8` — секциите използваха еднаква generic `.card`/`.content` структура без визуален anchor, `CbtModelDiagram` (обикновен 5-стъпков бутон-selector) не се възприема като "симулатор" (няма control panel/входни параметри/live output), `InterpretationExample` изискваше клик преди каквото и да е съдържание да се покаже (усещане за празна страница). `/kurs` — само линеен списък от карти, без визуална идентичност на 4-те блока, без усещане за маршрут. `/kpt` — диаграмата остава адекватна за целта си (KEEP), не пипната тази стъпка.
- **Код (ново, най-съществената промяна):** `CbtChainSimulator.razor` (Client) — реален simulator workspace: header с "Симулатор 01" badge, control panel (4 select-а: ситуация/мисъл/емоция/поведение + range slider за демонстрационен интензитет), live обновяван flow diagram (`aria-live`), detail panel, reset бутон. Затворени избори навсякъде — без личен свободен текст, без scoring/диагноза/AI/съхранение/сървърна заявка.
- **Код (пренаписано):** `InterpretationExample.razor` — от click-to-reveal единичен резултат към always-visible branch diagram (и двата пътя видими едновременно на desktop, вертикално на mobile), с добавена телесна реакция за всеки път, "Подчертай път А/Б" toggle (emphasis, не скриване).
- **Код (пренаписано):** `CategorizationCheck.razor` — от голи бутони към workspace с легенда (4 категории с описание), "Разгледани примери: N/4" (некомпетитивенброй), reset.
- **Код (пренаписано):** `Sedmica8.razor` — 8 визуално различими, anchor-навигируеми секции (Week Navigator, Накратко, Карта на темата [static concept map + факт-срещу-интерпретация/мисъл-срещу-чувство мини сравнения], Симулатор, Сравнение [branch diagram], Мисъл или емоция [comparison matrix + workspace], Пълно обяснение [реално разширено, не повторение], Проверка на разбирането, Обобщение/източници/disclaimer). Нов `.workspace` контейнер (по-широк от `.content`) за диаграмните/симулаторните секции.
- **Код (пренаписано):** `Kurs.razor` — добавена визуална course map (4 блока с connector стрелки, брой седмици), вертикална week timeline (номерирани markers, connector линии между седмиците), start panel, обясняващ че Седмица 8 е единствената налична демонстрация, а останалите 14 са roadmap.
- **UI UX Pro Max:** нови targeted заявки за simulator control panel/diagram-led sectioned learning — hover/input-affordance приети (вече покрити от съществуващите tokens); всичките 4 "dark"/decorative стилови резултата (OLED, Cinema Mobile, Cyberpunk, Claymorphism) отхвърлени трети път подред.
- **Тестове (добавено):** `SectionArchitectureTests.cs` (нов, 9 теста — 8 anchors, section nav, disclaimer-not-nested, 2 отделни disclosure блока, "пълно обяснение" реално по-дълго от краткото, workspace контейнер, concept map, comparison matrix), разширения в `InteractiveUiTests.cs` (+13, вкл. `CbtChainSimulator`) и `CurriculumHubTests.cs` (+4, course-map/timeline/start-panel/no-fake-progress). Общо 107/107 passing.
- **Проверено:** build 0/0; стар сървърен процес на порт 5055 спрян точково, рестартиран с новия build; реален HTTP smoke test — всички 8 секции + section-nav anchors присъстват, всички 4 ситуационни select опции реално prerendered, branch diagram/comparison matrix/categorization legend реално рендерени, 4 course-map блока + 15 week-timeline елемента на `/kurs`, dark/light theme запазени, без ECTS/forbidden distortion string, сървърен лог чист (само познатото HTTPS redirect warning).
- **Статус:** `CONTENT-RICH SIMULATOR REDESIGN READY — OWNER VISUAL REVIEW REQUIRED`. **Промените НЕ са commit-нати** — трети натрупан checkpoint върху некомитнатата Сесия 17+18 работа.

## 2026-08-01 (продължение 7) — Weekly Course Hub + Simulator Foundation (НЕКОМИТНАТО, върху Сесия 17)

- **Архитектура (ново постоянно решение, ADR-010):** `/kurs` Weekly Course Hub — 15-седмичен curriculum reference слой (4 блока), формална curriculum safety classification, progressive disclosure и симулатор-богато обучение като постоянни продуктови принципи. Пълни детайли, вкл. PDF-vs-Source-Register проверка (2 доклада конфликта, не примирени мълчаливо), в `03_DECISION_LOG.md`.
- **Source (ново):** `kpt_syllabus.pdf` прочетен изцяло (8 страници) — potвърждава 4-те curriculum блока и safety класификацията почти дословно; институционалното оформление ("Катедра по Клинична психология", "6 ECTS") изрично НЕ се възпроизвежда никъде в платформата.
- **Код (добавено):** `Curriculum/CourseCatalog.cs` + `CurriculumEnums.cs` — типизиран metadata модел (15 `CourseWeekDefinition` + 4 `CourseModule`), единствен source of truth, без lesson текст/лични данни/scoring.
- **Код (добавено):** `Kurs.razor` (`/kurs`) — публичен hub, честни статус етикети (Налично/В подготовка/Академичен обзор/Изисква професионален преглед), 0 dead links (само Седмица 8 с реален route).
- **Код (добавено):** `Sedmica8.razor` (`/kurs/sedmica-8`) — представителна седмица, пълен 8-секционен template, композира върху съществуващите 3 Модул 2 урока (не дублира текста им), `CbtModelDiagram`/`InterpretationExample`/нов `CategorizationCheck`.
- **Код (добавено):** `CategorizationCheck.razor` (Client, Interactive WebAssembly) — reveal-style проверка на разбирането (мисъл/емоция/телесна реакция/поведение), без scoring/правилен-грешен език.
- **Код (добавено):** `ProgressiveExplanation.razor` (host, static SSR, native `<details>`) — 3 реални употреби (Kurs intro + Sedmica8 пълно обяснение + академичен контекст).
- **Дизайн (пренаписано):** `MainLayout.razor`/`app.css` — sidebar+top-bar application shell (заменя STEP-2.1 header-nav shell), структурно вдъхновен от собственически предоставен reference (без брандинг/копиране/gamification/AI tutor). Съществуващите Модул 1/2 routes и клиничното съдържание непроменени.
- **Тестове (добавено):** `CurriculumHubTests.cs` (17 теста) + разширения в `InteractiveUiTests.cs` (4) + `ContentSliceTests.cs` (2 нови InlineData реда). Общо 81/81 passing.
- **Проверено:** build 0/0; реален HTTP smoke test на fresh инстанция (след PowerShell орфан-процес почистване) — всички routes 200, несъществуваща седмица → 404, 15 week-list елемента, всички нови интерактивни маркери реално prerendered (вкл. кирилски label-и на диаграмата и categorization-check фразите), light theme CSS override запазен, disclaimer/h1/forbidden-term проверки чисти, без ECTS/институционални claims в изхода.
- **Статус:** `WEEKLY HUB AND SIMULATOR FOUNDATION READY — OWNER VISUAL REVIEW REQUIRED`. **Промените НЕ са commit-нати** — приложението е оставено стартирано на `http://localhost:5055` за собственически визуален преглед на двата натрупани checkpoint-а заедно.

## 2026-08-01 (продължение 6) — Visual Direction Correction: dark-first UI + интерактивни учебни визуализации (НЕКОМИТНАТО)

- **Повод:** собственикът направи първи реален визуален преглед и отхвърли текущата визуална посока като не-финална. Изисквания: dark mode по подразбиране (не чисто черен), по-богата визуална йерархия, повече педагогически визуализации, повече смислена интерактивност, по-малко усещане за "серия статични текстови страници".
- **Дизайн (пренаписано):** `app.css` — тъмна тема като default `:root`, светлата тема преместена в `:root[data-theme="light"]` override (без промяна на стойностите от STEP-2.1); нови `--color-surface-elevated`/`--color-link` токена; всички цветови двойки реално проверени с WCAG contrast математика (Python скрипт, не на око) — виж `10_SESSION_LOG.md` за точните стойности.
- **Код (добавено):** `ThemeToggle.razor` (Interactive WebAssembly, сесийно състояние, без cookies/localStorage/server storage — light тема остава достъпна, dark остава default при refresh); JS interop (`theme.js`, 5 реда) за `data-theme` attribute toggle.
- **Код (добавено):** `CbtModelDiagram.razor` — интерактивна 5-стъпкова диаграма на модела (Ситуация→Мисъл→Емоция→Телесна реакция→Поведение), keyboard/mouse/touch, `aria-current="step"`, `aria-live="polite"` обяснение, `<details>` текстов fallback. Използвана на `/kpt` и Модул 2 Урок 1.
- **Код (добавено):** `InterpretationExample.razor` — малък интерактивен пример (неутрална ситуация, 2 алтернативни интерпретации, `aria-live` резултат), изрично без scoring/съхранение/грешен-правилен език. На `/kpt`.
- **Код (добавено):** `LearningPathVisualization.razor` — заменя плоския списък `ModuleCard` на Home/Programa с визуална последователност (2 налични модула + 1 честно "предстои" стъпка, без dead links, без fake progress).
- **Код (без промяна):** клинично съдържание непроменено навсякъде; thought record упражнение не е започнато (изрично забранено за тази стъпка).
- **Тестове (добавено):** `InteractiveUiTests.cs` (13 нови теста) + 2 актуализирани в `ContentSliceTests.cs` (Home/Programa вече проверяват `<LearningPathVisualization` вместо директен route string). Общо 56/56 passing.
- **Проверено:** build 0/0; реален HTTP smoke test на fresh `http://localhost:5055` инстанция — `data-theme="dark"` на initial `<html>`, всички нови компонентни маркери реално сервирани (вкл. prerendered WASM markup), light override в `app.css`, 0 disabled карти, disclaimer/h1/forbidden-term проверки непроменени навсякъде.
- **Реален технически проблем открит и коригиран:** orphaned `dotnet run` процеси от по-ранни smoke tests в тази сесия не бяха реално терминирани от bash `kill` (Windows child process, невидим за `ps` в Git-Bash/MSYS) — доведе до подвеждащ първи smoke test resultat срещу стар процес. Диагностицирано и коригирано чрез PowerShell (`Get-Process`/`Get-NetTCPConnection`/`Stop-Process`); документирано като процесен урок.
- **Статус:** `TECHNICAL QA COMPLETE — OWNER VISUAL APPROVAL PENDING`. **Промените НЕ са commit-нати** — изчакват собственически визуален преглед по изричната собственическа commit-стратегия за тази стъпка.

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
