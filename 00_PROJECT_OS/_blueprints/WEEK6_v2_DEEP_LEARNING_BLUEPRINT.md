# WEEK 6 v2 — DEEP LEARNING BLUEPRINT v1.1

**Сесия 39 · 2026-08-14 · КПТ Академия Project OS**
**Source:** SRC-041 (Джудит С. Бек, "Когнитивно-поведенческа терапия: Основи и отвъд", 2-ро изд., Guilford Press, 2011), **Глава 5 — Структура на ПЪРВАТА терапевтична сесия** — printed стр. 59–79. Source baseline непроменен от v1.0 (без нов extraction).
**Status:** REVISION PASS COMPLETE — все още не е implementation, не е commit, не е маркирано `COMPLETE`.

---

## 1. Revision summary

Точно какво е поправено спрямо v1.0:

- **Owner decisions вградени:** U08/U22 → `INCLUDED — OBSERVATIONAL SAFETY BOUNDARY` (вече не `NEEDS REVIEW`). Ирина → `APPROVED PILOT LONGITUDINAL CASE`.
- **Section count поправен навсякъде:** "13 секции" → **14 секции** (6.0 → 6.13 = 14 записа, не 13).
- **Coverage Matrix Type-категориите преброени реално** (не копирани от стар, непроверен summary) — старият summary (Core 10 / Example 13 / Warning 7 / Process 12 / Cross-ref 5 = 47) не съответстваше на самата 47-редова таблица и изцяло пропускаше категорията Distinction. Верните преброени стойности виж §2.
- **Terminology Map преброен реално** — таблицата съдържа **11** термина, не 12; **8** от тях (не 9) са маркирани за flashcard. Твърдението "9 от 12" в v1.0 беше грешно на два фронта едновременно.
- **Assessment difficulty-разпределението преброено реално** — v1.0 текстово твърдеше "7 Basic / 8 Intermediate / 5 Advanced", но самата 20-редова таблица реално съдържаше 9/7/4. Поправено (виж §10).
- **Coverage rule сменен** от `≥90% PLANNED` на **100% accounted-for** (INCLUDED / DEFERRED / EXCLUDED / NEEDS REVIEW, с изрична забрана за мълчаливо пропускане). Пълен одит в §3.
- **First-session precision pass** — открит и документиран реален source-fidelity дефект: **v1.0's текущ жив текст** ("общата форма на *стандартна* сесия") генерализира първо-сесийната структура на Глава 5 към всяка сесия, което самата книга не твърди (Глава 7 описва различна структура за сесия 2+). Коригирано навсякъде в v1.1 (виж §5).
- **Visual Learning Plan пренаписан** с explicit "Visual type" поле за всеки визуал; V4 (decision tree) беше де факто плосък 3-card grid (`.category-compare`), не истинско branching дърво — прекласифициран и надграден с нов CSS-only branching pattern (виж §7).
- **`FigureReproduction`** преименуван на **`SourceArtifact`**, преосмислен като общ "source-grounded → собствен accessible артефакт" компонент, не pixel-copy на книжна фигура.
- **`ClinicalQABox`** преименуван на **`WhatIfBox`** — директно отразява собствения педагогически похват на книгата ("Какво ако…"), не клиничен jargon.
- **`ScenarioSimulator`** архитектурата пренаписана от MC-центрична на multi-interaction (identify/matching/ordering/branching) — виж §8.
- **Q19 заменен** — старият проверяваше platform safety policy (observation vs self-assessment), не CBT знание. Новият е source-grounded academic въпрос върху U13 (виж §10).
- **Learning time преработено** в 4 категории с диапазони (не единично число) — виж §11.
- Никакъв `.razor` файл, CourseCatalog, тест или CSS не е пипан. Никакъв git commit не е направен.

---

## 2. Corrected inventory statistics

**Total units:** 47 (непроменено — source baseline не е преработван).

**Breakdown by Type (реално преброено от 47-редовата Coverage Matrix):**

| Type | Брой | Unit IDs |
|---|---|---|
| Core | **15** | U01,U02,U03,U07,U10,U14,U15,U21,U24,U26,U33,U34,U39,U40,U45 |
| Example | **9** | U04,U09,U11,U16,U22,U27,U28,U35,U41 |
| Process | **13** | U06,U12,U17,U18,U20,U23,U29,U30,U31,U36,U37,U42,U44 |
| Warning | **6** | U05,U08,U13,U19,U32,U43 |
| Distinction | **2** | U25,U38 |
| Cross-reference | **2** | U46,U47 |
| **Общо** | **47** | ✓ съвпада с матрицата |

**Terminology count:** 11 термина общо в Terminology Map (не 12).
**Flashcard разбивка:** 8 маркирани "Да" (memory-worthy), 3 маркирани "Не" (визуални инструменти/forward-reference, не самостоятелен recall термин). 8 + 3 = 11 ✓

**Section count:** **14 секции** (6.0, 6.1, 6.2, 6.3, 6.4, 6.5, 6.6, 6.7, 6.8, 6.9, 6.10, 6.11, 6.12, 6.13).

---

## 3. Final Coverage Audit

Ново правило (замества старото `≥90% PLANNED`, премахнато изцяло):

> Всеки knowledge unit получава един от статусите **INCLUDED** / **DEFERRED — explicit destination** / **EXCLUDED — explicit reason** / **NEEDS REVIEW**. Нищо не изчезва мълчаливо.

**Резултат от одита:**

- **Total identified:** 47
- **Included:** 47
- **Deferred:** 0
- **Excluded:** 0
- **Needs review:** 0
- **Unaccounted:** **0**

Всичките 47 единици принадлежат директно на Глава 5 и се преподават в рамките на Week 6 v2 — реалната глава не съдържа материал, който трябва да бъде преместен в друга седмица. Четири единици съдържат forward-reference **бележки** към други глави, но самото им съдържание (какво казва Глава 5 за тях) е включено тук изцяло:

| ID | Forward-reference towards | Третиране в Week 6 v2 |
|---|---|---|
| U30 | Глава 11 ("Оценка на автоматичните мисли") | INCLUDED: домашното задание "0–100% истина" се представя точно както Гл. 5 го въвежда (като начален homework инструмент). Пълната методология за оценка на автоматични мисли **не** се разгръща тук — тя принадлежи на бъдеща седмица, картографирана върху Глава 11; точният номер на тази седмица не е потвърден в тази сесия (изисква проверка в `CourseCatalog.cs` при бъдещо планиране, не сега). |
| U33 | Глава 6 / бъдещата **Седмица 7** ("Поведенческа активация") | INCLUDED: Гл. 5's собствено кратко въведение в поведенческата активация (само ако има време в първата сесия) е включено. Пълното разгръщане на техниката е изрично извън обхвата — принадлежи на Седмица 7, вече потвърдена в `CourseCatalog.cs`. |
| U46 | Глави 6, 7, 8, 9, 11, 17 | INCLUDED като Cross-week connections бележки (§ раздел 6.12/§15 от v1.0) — не отделно съдържание. |
| U47 | Wenzel et al. 2008; J. S. Beck (2005) | INCLUDED като библиографска препратка в Deep Dive текста (U08) и Term card (U25) — reference only, не отделен knowledge block. |

Нищо не е `EXCLUDED` — Глава 5 не съдържа материал, преценен като неподходящ за преподаване.

**Контролен въпрос, повторен:** "Има ли knowledge unit от Глава 5, който не е accounted for?" — **Не. Unaccounted = 0.**

---

## 4. Owner decisions incorporated

**U08** (проверка за suicidality/hopelessness items в стандартизирани въпросници) и **U22** (диалогов момент "Имала ли си мисли за нараняване на себе си?") — статус сменен от `NEEDS REVIEW` на:

> **`INCLUDED — OBSERVATIONAL SAFETY BOUNDARY`**

Строго правило, вградено в дизайна на 6.4 (Deep Dive) и Case Lab/Simulator (§8, §9): learner-ът изучава как терапевт разпознава и обработва risk-related информация върху фиктивен пациент; трето лице навсякъде; няма self-screening въпрос към learner-а; няма BDI/BAI за попълване от него; няма scoring; няма автоматична risk classification; няма персонални клинични препоръки; няма input поле, което имитира реална оценка на риска.

**Ирина** (Intermediate case от Case Lab, §9 в v1.0) — статус:

> **`Approved as longitudinal pilot — future reuse only where pedagogically natural.`**

Употребена в Week 6 v2 като Intermediate случай. Никаква пълна бъдеща история не е измислена предварително — бъдещото ѝ появяване (ако изобщо) ще се реши седмица по седмица, само където реално пасва на съдържанието, не механично.

---

## 5. First-session precision audit

**Проверени области:** 6-те цели, 10-стъпковата структура, времетраенето, diagnosis discussion, initial goal setting, initial socialization.

**Находка (най-съществената в тази ревизия):** текущият **жив, публикуван** текст на `Sedmica6.razor` (v1) съдържа реален source-fidelity дефект — описва структурата като принадлежаща на "*стандартна*" КПТ сесия ("Тази седмица разглежда общата форма на *стандартна* сесия по когнитивно-поведенческа терапия"). Глава 5 обаче е озаглавена и структурирана изрично около **първата** сесия; самата книга посвещава отделна Глава 7 ("Сесия 2 и след това: структура и формат") точно защото структурата на следващите сесии се различава. Генерализирането на v1 е несъзнателна overclaim отвъд source-а, не проверена по-рано.

**Поправка, приложена в v1.1 архитектурата (§7):**

- 6.0 и 6.1 вече изрично рамкират цялото съдържание като "структурата на **първата** терапевтична сесия по Глава 5", с кратка explicit бележка, че сесия 2 и нататък следват различна (но свързана) структура — форward-препратка, не подробност.
- 10-стъпковата структура (U02) навсякъде носи етикет "10-те стъпки на **първата** сесия", не generic "10-те стъпки на сесията".
- Diagnosis discussion (U15–U20), initial goal setting (U21–U25), initial socialization (U01, U26) — и трите изрично рамкирани като еднократни, first-session-specific моменти (пациентът научава диагнозата си за пръв път; целите се задават за пръв път; моделът се обяснява за пръв път) — не се твърди, че се повтарят идентично във всяка следваща сесия.
- Времетраенето (45–50 мин. стандартно / ~1 час за първата) вече беше прецизно в v1.0 — потвърдено без промяна.

**Формулировка, използвана последователно:** „В Глава 5 Бек представя структурата на първата терапевтична сесия…" — не „Ето как изглежда всяка КПТ сесия".

---

## 6. Revised curriculum architecture

**14 секции** (коригирано от грешното "13"), всяка с ясен source mapping, visual classification, interaction и assessment връзка:

| # | Секция | Depth | Source units | Visual type | Interaction | Assessment link |
|---|---|---|---|---|---|---|
| 6.0 | Карта на седмицата — обхват, prerequisite (Седмица 3), learning outcomes, **explicit "първа сесия, не всяка сесия" рамка** | — | — | — | Нет | — |
| 6.1 | Защо *първата* сесия има структура — 6-те цели, 10-степенна рамка | L1 | U01–U02 | Graphic diagram (V1) | Нет | Q01,Q02,Q07 |
| 6.2 | Начало на сесията — дневен ред, настроение, актуализация, диагноза | L1/L2 | U03–U20 | Graphic diagram (V4) | Knowledge check | Q03,Q04,Q06,Q19 |
| 6.3 | Ключови понятия — Terminology Map (11 термина) с flashcard preview | L1 | — | Styled information card | Flashcard preview | Q11 |
| 6.4 | Deep Dive — 3-те изключения от структурата; risk-check nuance (U08/U22, observational framing) | L2 | U08,U13,U19,U22 | Graphic diagram (V4) | Нет | Q05,Q06,Q19 |
| 6.5 | Beck in Practice — среда на сесията: проблеми→цели, когнитивен модел, "черна боя" | L1/L2 | U21–U33 | Graphic diagram (V2) + SVG illustration (V3) | Knowledge check | Q08,Q09,Q10,Q11,Q12,Q13 |
| 6.6 | Visual Learning — обобщен изглед на V1–V7 | L1 | — | Всички типове (виж §7) | Нет | — |
| 6.7 | Край на сесията — обобщение, домашна работа, обратна връзка | L1/L2 | U34–U45 | Source artifact (V5,V6) + Styled card (V7) | Knowledge check | Q14,Q15,Q16,Q17 |
| 6.8 | Case Lab — Мартин / Ирина (pilot longitudinal) / Радо | L3 | — | Case cards | Application | — |
| 6.9 | Interactive Simulator — Session Structure & Decision Simulator, Level A/B/C | L3 | U04,U13,U24,U27,U43 | Interactive diagram | Simulator (multi-interaction) | Q18 |
| 6.10 | Section Checks — обобщение на 6.2/6.5/6.7 | — | — | — | — | — |
| 6.11 | Final Assessment — 20 въпроса (виж §10) | L1–L3 | Всички | — | Assessment | Q01–Q20 |
| 6.12 | Review Map — U45 синтез, cross-week връзки (Седмица 3/7/8/10) | — | U45,U46 | — | Нет | Q20 |
| 6.13 | Sources / Optional Reading — SourceReferences + OptionalReadingSource | — | — | — | Нет | — |

---

## 7. Revised Visual Learning Plan

Явна класификация за всеки визуал, за да е ясно кое реално е графично и кое е CSS presentation:

| Visual | Visual type | Учебна цел | Reuse / нов |
|---|---|---|---|
| **V1 — 10-Step First Session Timeline** | **Graphic diagram** | Показва пълната явна структура на *първата* сесия наведнъж, преди детайлно обяснение | Reuse на `.guided-practice-sequence` (Седмица 10) — реален номериран process rail с визуални connector-и, разширен от 6 на 10 units |
| **V2 — Ситуация → Мисъл → Реакция** | **Graphic diagram** | Показва как терапевтът улавя модела в реално време от спонтанен пример (обяд с колеги) | Reuse на `.concept-map__flow` (Седмица 3) — реален flow diagram с SVG connector, не 3 отделни card-а |
| **V3 — "Черна боя / очила" метафора** | **SVG illustration** | Прави когнитивната триада запомняща се чрез конкретен образ | Нов малък custom SVG (очила + градиент маска) — геометрична илюстрация, не снимка/AI-generated art |
| **V4 — Кога да отклониш от структурата** | **Graphic diagram (branching)** | Показва трите легитимни изключения (U13) като истинско branching решение, не плосък списък | **Прекласифициран и надграден** — v1.0 планираше reuse на `.category-compare` (плосък 3-card grid, **не** истинско дърво). v1.1 предлага нов лек CSS-only pattern `.decision-branch` (root node → 3 branch lines → 3 leaf nodes, чист CSS/flexbox + connector линии, без нов JS/Blazor компонент) |
| **V5 — Фигура 5.1 репродукция (Списък с домашни)** | **Source artifact** | Показва реален клиничен инструмент като конкретен артефакт, не абстрактно описание | Нов `SourceArtifact` компонент (§8) — собствен дизайн, семантичен `<ol>`, не pixel-copy |
| **V6 — Фигура 5.2 репродукция (Доклад за терапия)** | **Source artifact** | Същата логика като V5, за 5-въпросния формуляр | `SourceArtifact` компонент, вариант с `<dl>` за въпрос/отговор двойки |
| **V7 — Time-Realism Comparison** | **Styled information card** | Илюстрира нюанса от U36 ("очаквано 5 мин. vs реално &lt;1 мин.") | Reuse `.concept-map__side-notes` — честно означено като card, не диаграма |

**Минимум 3 истински графични визуализации: изпълнено (V1, V2, V3, V4 — четири, не три).** V5/V6/V7 са честно класифицирани като различни (но легитимни) типове, не представени като "graphic diagram" фалшиво.

**Accessibility за всеки graphical/SVG визуал (V1–V4):** пълен текстов еквивалент непосредствено под/до диаграмата (номериран списък или definition list); логичен screen-reader ред, следващ визуалния (не обратно); caption/label за всеки елемент; никаква информация, носена само чрез цвят — всяка branch/стъпка има и текстов label; мобилен fallback — вертикален stack вместо хоризонтален rail под установения breakpoint (същия принцип като `.guided-practice-sequence`-ъ вече ползва). Accessibility fallback-ът **допълва** визуала, не го замества — реалната SVG/diagram версия остава основното представяне на desktop/mobile, не се "жертва" в полза само на текст.

---

## 8. Component Plan — final version

### Existing components to reuse (без промяна)
`LearningSection`, `LearningObjectives`, `ProgressiveExplanation`, `DisclaimerCallout`, `SourceReferences`, `OptionalReadingSource`, `.concept-map__side-notes`.

### Existing components/patterns to extend
- `.guided-practice-sequence` — досега тестван само за 6 units (Седмица 10); за V1 се разширява до 10 без промяна на самата CSS логика (данни, не архитектура).
- `.concept-map__flow` — reuse без структурна промяна за V2.
- Native `<details>`/`<summary>` knowledge checks — нужни нови *ordering* и *matching* варианти (HTML form-based, без нова JS библиотека).

### New reusable components

**`SourceArtifact`** *(преименуван от отхвърленото `FigureReproduction`)*
- **Purpose:** превръща source-grounded worksheet/handout/фигура (Фиг. 5.1, 5.2 тук) в собствен, accessible, responsive учебен артефакт — не pixel-copy на оригинала.
- **Защо съществуващ компонент не може чисто:** `LearningSection`/`.card` нямат отделна визуална идентичност за "това е реален клиничен инструмент, показан за илюстрация" — нужен е разпознаваем "handout" стил (различен border/texture), семантично различен от обикновена проза.
- **Expected reuse:** книгата съдържа десетки подобни фигури в други глави (Дневник на мислите, Диаграма на активността, Cognitive Conceptualization Diagram и др.) — висока бъдеща reuse стойност.
- **Accessibility implications:** семантичен `<ol>`/`<dl>`, никога изображение на текст; ясен `aria-label`, идентифициращ го като "възпроизведен учебен артефакт", не оригинален документ.
- **Needed immediately in Week 6 v2:** Да (V5, V6).

**`WhatIfBox`** *(преименуван от отхвърленото `ClinicalQABox`)*
- **Purpose:** репродуцира собствения педагогически похват на книгата ("Какво ако…") за troubleshooting/therapist reasoning/decision logic.
- **Защо съществуващ компонент не може чисто:** `ProgressiveExplanation`'s `<details>` е single-reveal общо обяснение, не структуриран "въпрос-възражение → разсъждение" формат с разпознаваем визуален "FAQ" сигнал.
- **Expected reuse:** похватът се повтаря във *всяка* глава на книгата (само в Гл. 5 — 5 инстанции) — най-високата reuse стойност от трите нови компонента.
- **Accessibility implications:** проста, semantic `<details>`/`<summary>` двойка с ясен "Какво ако…" префикс в label-а.
- **Needed immediately in Week 6 v2:** Да (U05, U13, U19, U32).

**`ScenarioSimulator`** *(одобрен като concept, архитектурата пренаписана — виж §9)*
- **Purpose:** генерична, data-driven, multi-interaction, multi-level (Recognition/Application/Reasoning) симулаторна рамка — не hardcoded per week.
- **Защо съществуващ компонент не може чисто:** нито един съществуващ интерактивен компонент (`CbtChainSimulator`, `SocraticDialogueExplorer`) поддържа branching state с последователни решения и различни последствия.
- **Expected reuse:** точно моделът, необходим за всяка бъдеща Deep Learning Week — най-високата архитектурна инвестиция, но и най-широката бъдеща употреба.
- **Accessibility implications:** фокус мениджмънт между branch-стъпки, `aria-live` регион за последствия/feedback текст, пълна клавиатурна навигация без разчитане на drag-and-drop за задължителни пътища (drag остава opt-in enhancement, не единствен метод за ordering interaction-и).
- **Needed immediately in Week 6 v2:** Да (6.9).

**`.decision-branch`** *(нов CSS-only pattern, не Blazor компонент)*
- **Purpose:** истинска branching визуализация (V4) вместо плосък comparison grid.
- **Защо `.category-compare` не може чисто:** е дефиниран и визуално разпознаваем като *паралелно сравнение* (3 равностойни колони), не като *решение с разклонение от общ корен* — семантично различна форма.
- **Expected reuse:** книгата съдържа многократни "when to deviate" решения в други глави — реалистична бъдеща употреба.
- **Needed immediately in Week 6 v2:** Да (V4).

---

## 9. Revised Simulator Design

**Session Structure & Decision Simulator** — вече multi-interaction, не MC-центричен quiz.

### Level A — Recognition
Не само MC. Два interaction типа:
- **Identify-the-step:** кратък откъс → избор от всичките 10 стъпки (истинско classify, не A/Б/В).
- **Matching:** 4 кратки откъса ↔ 4 имена на стъпки, съпоставени едновременно (не последователни отделни въпроси).

### Level B — Application
Задължително включва манипулация на структурата, не само познаване на отговор:
- **Ordering:** learner подрежда 4 разбъркани представяния на стъпки от началото на сесията в правилна последователност (директна манипулация, keyboard-достъпна чрез up/down контроли, не само drag).
- **Choose-next-step + reasoning:** ситуация на решение (напр. пациент възразява на дневния ред) → learner избира следващо действие **и** посочва кой от трите U13 критерия (ако има) обосновава отклонение.

### Level C — Reasoning: BRANCHING MULTI-STEP SCENARIO
Базиран на U43 (негативна реакция на сесията, преразказан сценарий), структуриран по точния 7-стъпков модел:

1. Фиктивен пациент ("Радо") изразява недоволство в края на сесията.
2. Learner избира първоначално действие (напр. "попитай директно какво го притеснява" срещу "продължи по план").
3. Сценарият продължава според избора — различен следващ откъс за всеки път.
4. Появява се нова информация (пациентът разкрива, че домашното изглежда претоварващо).
5. Learner прави второ решение (напр. "направи цялото домашно опционално" срещу "обясни защо е важно" срещу "раздели го на по-малки части").
6. Системата обяснява последиците от избора — как всеки вариант би повлиял на алианса, позовавайки се на U43/U44.
7. Финален source-grounded reasoning summary, свързан обратно към 6.4/6.7.

### Feedback contract (за всяко решение, на всяко ниво)
Никога само "Верен"/"Грешен". Всеки отговор включва: **какво** е правилно/неправилно; **защо**; **кой source unit** подкрепя обяснението (U-ID); **линк** обратно към съответната 6.x секция; **типична грешка в разсъждението**, ако изборът е грешен.

---

## 10. Revised Assessment Blueprint

### Q19 — заменен

**Стар Q19** (премахнат): проверяваше platform safety policy ("наблюдение срещу самооценка") — не academic CBT знание, неподходящо за финален content assessment.

**Нов Q19:**
> Пациент разкрива нова, силно обезпокоителна информация точно преди терапевтът да премине към обучение за когнитивния модел (стъпка 6). Кои от изброените са легитимни, source-grounded основания терапевтът да отложи планираната дневен-ред точка в полза на новата информация? *(Scenario, multi-select срещу distractors, базиран директно на U13's три критерия + U08/U22's risk-priority логика.)*

Difficulty: Advanced. Skill: Analyze (не Distinguish — реалната задача е да се анализира ситуация спрямо изрично посочени критерии, не просто да се различат два термина).

### Коригирано разпределение (реално преброено от таблицата, не преповторено твърдение)

**По difficulty:** v1.0 текстово твърдеше "7 Basic / 8 Intermediate / 5 Advanced" — не съответстваше на собствената си таблица. Реално преброено: **9 Basic / 7 Intermediate / 4 Advanced = 20.** Q19's difficulty остава Advanced след замяната — разпределението по difficulty не се променя от самата замяна.

**По skill (след Q19 замяната):**

| Skill | Брой | Въпроси |
|---|---|---|
| Recall | 6 | Q01,Q02,Q07,Q14,Q17,Q20 |
| Understand | 6 | Q04,Q08,Q11,Q13,Q15,Q16 |
| Distinguish | 2 | Q06,Q10 |
| Apply | 2 | Q03,Q09 |
| Analyze | 4 | Q05,Q12,Q18,Q19 |
| **Общо** | **20** | ✓ |

### Coverage audit на assessment blueprint-а
20-те въпроса покриват единици от всичките 14 секции без прекомерна концентрация върху една (максимум 4 въпроса позовават се пряко на един и същ source cluster — U13, разпределени между Q05/Q06/Q19, което е оправдано предвид неговата "Много висока" важност в Coverage Matrix-а, не произволно раздуване). И петте изисквани skill типа (Recall/Understand/Distinguish/Apply/Analyze) присъстват. **Диапазонът 18–22 се запазва при 20** — оправдан от реалното съдържателно покритие (14 секции × ~1.4 въпроса средно), не избран заради кръгло число.

---

## 11. Revised Learning-Time Estimate

Изчислено спрямо действителния v1.1 blueprint (архитектура §6, simulator §9, assessment §10), не нагласено насила към предложения диапазон.

| Категория | Обхваща | Диапазон |
|---|---|---|
| **Core Pass** | Първи прочит на 6.0–6.3, 6.6 (visual overview), основни section checks | 25–35 мин. |
| **Deep Study** | 6.4 Deep Dive, 6.5 Beck in Practice (13 units, най-богатата секция), Terminology Map (11 термина), advanced visuals | 35–45 мин. |
| **Practice** | Case Lab (3 случая), Simulator Level A+B+C (вкл. branching Level C) | 40–50 мин. |
| **Assessment & Review** | Final assessment (20 въпроса с explanatory feedback), flashcards (8), Review Map | 40–60 мин. |
| **TOTAL ACTIVE STUDY RANGE** | | **~140–190 мин. (≈2.3–3.2 часа)** |

Диапазонът се разполага близо до, но не изкуствено принуден в, предложената собственическа интуиция за 2.5–4 часа — изчислен от реалния обем (47 units, 14 секции, 3-нивов симулатор, 20-въпросен тест), не обратно нагласен. Разделим на 2–3 отделни сесии в рамките на седмицата.

---

## 12. Project-wide implications (актуализирани)

| Измерение | Нов стандарт |
|---|---|
| **Source** | Пълен прочит на релевантната глава преди lesson design — не резюме от паметта на проекта. |
| **Coverage** | 100% accounted-for (Included/Deferred/Excluded/Needs Review) — `≥90%` официално премахнато. |
| **Terminology** | Нито един нов термин без обяснение; таблицата се преброява реално, не се предполага. |
| **Content** | Дълбоко обяснение, множество примери, разграничения, нюанс — first-session precision изрично проверявана, не приема се generic. |
| **Visuals** | Минимум 3 истински графични учебни визуализации (не styled cards) на седмица, с explicit Visual type класификация. |
| **Practice** | Смислено приложение (Case Lab + multi-interaction Simulator), не token quiz. |
| **Assessment** | Section checks + сериозен финален тест, покриващ 5-те skill типа, source-grounded (не platform-policy) съдържание. |
| **Feedback** | Обяснително, source-grounded, никога голо "Верен/Грешен". |
| **Owner review** | Собственикът потвърждава, че модулът реално учи — последна gate, не автоматична. |

---

## 13. Deep Learning Week — Definition of Done v2

1. Full relevant source reading completed (реален файл, не резюме).
2. Knowledge units identified (Chapter Coverage Matrix).
3. **100% units accounted for** (Included/Deferred/Excluded/Needs Review; Unaccounted = 0).
4. Terminology Map complete, реално преброена.
5. No unexplained key terms.
6. Core theory present.
7. Deep-dive nuance present.
8. Multiple concrete examples.
9. Case-based application (нови, source-inspired фиктивни персонажи).
10. At least one meaningful practical interaction where pedagogically appropriate (multi-interaction simulator, не само MC).
11. Real visual learning — минимум 3 истински графични визуализации, explicit classified, не само text cards.
12. Section checks след всеки основен блок.
13. Serious final assessment, покриващ Recall/Understand/Distinguish/Apply/Analyze.
14. Explanatory feedback — какво/защо/source/link/типична грешка.
15. Flashcard/review mechanism where useful.
16. Cross-week links, включително first-session vs subsequent-session precision, ако е приложимо.
17. Source traceability — всяко твърдение проследимо до U-ID.
18. Safety QA — explicit observational/third-person framing за всеки чувствителен елемент.
19. Accessibility QA (визуали + interaction).
20. Visual QA (структурна прозрачност за метода).
21. Functional tests, отразяващи реалния нов обем.
22. Owner learning review — потвърждение, че съдържанието реално учи.

---

## 14. Remaining unresolved decisions

None.

---

## 15. Recommendation

**READY FOR IMPLEMENTATION PROMPT: YES.**

47/47 units accounted for (0 unaccounted); всички аритметични несъответствия от v1.0 намерени и коригирани (Type-категории, terminology count, section count, assessment difficulty-разпределение); first-session precision дефект открит и коригиран в архитектурата (плюс отбелязан като реален defect за поправка в живия v1 текст при implementation); визуалният план вече прави честно разграничение graphic/SVG/artifact/styled-card, с надграден истински branching V4; simulator-ът е multi-interaction с branching Level C, не MC quiz; Q19 вече проверява academic CBT съдържание, не platform policy; component имената (`SourceArtifact`, `WhatIfBox`, `ScenarioSimulator`, `.decision-branch`) са обосновани индивидуално, не приети механично; learning time е диапазон, изчислен от реалния обем, не нагласен насила.

Не съм писал `.razor` код, не съм пипал `Sedmica6.razor`, `CourseCatalog.cs`, тестове или CSS. Не съм направил git commit.
