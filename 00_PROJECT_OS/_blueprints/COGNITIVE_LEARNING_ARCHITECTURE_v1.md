# PROJECT-WIDE COGNITIVE LEARNING ARCHITECTURE v1.1

**Сесия 41 · 2026-08-21 · КПТ Академия Project OS**
**Revision pass върху:** v1 (Сесия 40) — статус беше `APPROVED IN PRINCIPLE — REVISION REQUIRED BEFORE IMPLEMENTATION`.
**Обхват на тази сесия:** Specification revision само. Никакъв implementation код, никаква промяна по Week 6/7/12, никакъв `.razor`/CSS/тест/`CourseCatalog.cs` файл не е пипнат. Няма git commit.
**Status:** REVISION PASS COMPLETE — все още не е implementation, не е commit, не е маркирано `COMPLETE`.

**Фундаментална корекция, движеща тази ревизия:**

> **COURSE MAP показва къде учим.**
> **KNOWLEDGE MAP показва какво знаем и как идеите са свързани.**
> **WEEKLY MIND MAP показва структурата на знанието в конкретната седмица.**
> **CONCEPT MAP показва отношенията между понятия.**
> **CASE CONCEPTUALIZATION MAP показва как тези понятия се проявяват в един фиктивен случай.**
>
> Те споделят rendering infrastructure. Те НЕ са едно и също нещо.

v1 смеси първите две в едно "Global CBT Master Mind Map". Това е основната грешка, поправена тук.

---

## 1. Revision summary

Точно какво е променено спрямо v1 (запазените силни части са изброени накрая):

1. **Course Map и CBT Knowledge Map разделени концептуално** (§2/§3) — v1's единствена "Global CBT Master Mind Map" (седмица-като-node) е сгрешено смесване на навигационна и знаниева структура. Седмица/Модул вече са **метаданни** върху concept nodes (`IntroducedWeek`/`RevisitedWeeks`), не равностойни nodes в knowledge graph-а.
2. **`/kurs/karta` остава един route, но с два ясно различими режима** (§2/§3/§9), не два отделни семантични модела на едно и също ниво.
3. **Weekly Mind Map коригиран** (§4) — nodes вече са knowledge clusters/concepts на седмицата (напр. "agenda setting", "mood check", "diagnosis discussion"), не визуален Table of Contents от секции. Anchor-ите (`#6.2`, `#6.5`) са navigation metadata върху node-а, не самият node.
4. **Preview и Review вече са две състояния на една и съща knowledge структура** (§4), не две различни карти.
5. **`ConceptGraph.razor` потвърден като rendering engine, но НЕ като единствен семантичен модел** (§8) — три отделни domain модела (`MindMapModel`, `ConceptMapModel`, `CaseConceptualizationModel`, §5/§6/§7) захранват renderer-а през adapter слой, вместо generic `GraphNode(Id,Label)`/`GraphEdge(A,B)` да стане de facto единствената истина.
6. **Course Map получава собствен model path** (§2) — работи директно с вече съществуващите `CourseWeekDefinition`/`CourseModule`, не се насилва в knowledge-map семантика.
7. **Curriculum-state semantics преименувани** (§12) — `Locked`/`Available`/`Reinforced` → `Upcoming`/`Introduced`/`Revisited`. Никога `Mastered`/`Learned`/`Completed` — платформата няма persistent learner-progress engine и не бива да намеква, че има.
8. **`ReinforcesWeekNumbers` разделено на две различни, непресичащи се полета** (§10/§2) — `CourseWeekDefinition.BuildsOnWeekNumbers` (week-to-week curriculum relation, за Knowledge Map cross-links) и `CourseWeekDefinition.PrerequisiteWeekNumber` (week-to-week navigation ordering, за Course Map) — v1's едно поле объркваше двете различни функции.
9. **Global Knowledge Map вече смее да показва `Upcoming` бъдещи концепти** (§3/§12) — приглушено, без дефиниция, без relationships — само като orientation landmark. Weekly Mind Map и Case Map остават строго без forward leak (§9's progressive-disclosure граница остава, но вече е ясно, че важи различно за различните карти, не еднакво за всички).
10. **Retrieval Practice издигнат в самостоятелна architecture секция** (§13), с изрично разграничение от recognition (§14) — `<details>`/`<summary>` вече не се брои автоматично за retrieval, освен ако не предхожда recall/reconstruct/predict/order/explain подкана.
11. **Нова Recognition/Retrieval/Application/Reasoning таксономия** (§14) — позволява реален audit на learning depth, илюстриран с реален (не измислен) одит на Седмица 6.
12. **DoD v3 вече има ДВЕ нови gates**, не една (§16/§17/§18) — Cognitive Representation Coverage (без механични количества) + нов Retrieval Practice Coverage gate.
13. **Memory Anchor "maximum ~1 на седмица" премахнато** (§16) — заменено с "само когато има ясна mnemonic function"; 0, 1 или 2 могат да бъдат напълно правилни.
14. **No-graph-library решението преформулирано като "current implementation decision"** (§21), с изричен revisit trigger, не вечна забрана.
15. **Owner decisions затворени** (§25): `/kurs/karta` permanent navigation; Static SSR by default + progressive WASM; build order Phase 1–7 (Week 6 reference преди global rollout, Week 7 остава frozen); retroactive metadata само след Week 6 reference approval, source-confirmed.
16. **Relationship vocabulary** (§11) — минимален, изведен от реално прочетено съдържание, не измислен предварително.

**Запазено без промяна от v1** (силните части, изрично защитени от заявката): diagnosis-а (§1 на v1 — Седмица 6 v2 остава диагностичният пример); cognitive-learning принципите; mind map ≠ concept map разграничението (сега формализирано архитектурно, не само декларативно); representation-fit принципа; no-new-library-before-necessity; accessibility-by-construction; longitudinal case подхода (Ирина); guided discovery; retrieval механизмите; `ScenarioSimulator` интеграцията; existing CSS/component reuse философията; Седмица 6 като reference implementation; local-first/no-persistent-clinical-data границата.

---

## 2. Course Map architecture

**Въпрос, на който отговаря:** „Къде се намирам в курса?"

**Структура:** точно `CourseCatalog.Modules` (4) → `CourseCatalog.Weeks` (15) — вече съществуващи, одобрени в `18_INFORMATION_ARCHITECTURE.md`. Никаква нова таксономия.

**Модел:** Course Map работи **директно** с вече съществуващите типове — `CourseWeekDefinition`/`CourseModule` — не се насилва през Knowledge Map семантика (§15 от заявката, спазено буквално). Полета, вече налични и достатъчни: `Number`, `ModuleLabel`, `Title`, `Route`, `Status` (`CourseWeekStatus`, непроменен), `LearningObjectives`.

**Две малки нови, чисто навигационни полета** (не concept semantics):

```
CourseWeekDefinition.PrerequisiteWeekNumber : int?   // "препоръчано е да сте минали Седмица X" —
                                                       // формализира прозата, вече използвана в 6.0/10.0
```

`Number`-редицата (1–15) вече Е педагогическата curriculum sequence — не се добавя отделно поле за нея, само се потвърждава, че съществуващото номериране изпълнява тази роля.

**Отделно, за Knowledge Map cross-links (не Course Map)** — виж §10 за пълния контраст.

**Rendering:** Course Map може да използва **същия** `ConceptGraph.razor` renderer (§8) през собствен `CourseMapAdapter`, ако това е чист reuse — но никога споделяйки `ConceptNode`/`ConceptMapModel` семантика. Layout: Mind-Map-стил tree (Modules → Weeks), не network — курсовата структура е строга йерархия, не мрежа от отношения.

---

## 3. CBT Knowledge Map architecture

**Въпрос, на който отговаря:** „Как са свързани идеите в КПТ?"

**Nodes са CONCEPTS, не седмици.** Седмица/Модул стават **метаданни върху concept node-а** (`IntroducedWeek`, `RevisitedWeeks`, `AnchorRoute`), не отделен, равностоен node между "автоматична мисъл" и "емоция".

**Концепти, реално изведени от прочетеното curriculum/source съдържание** (не примерния списък от заявката, макар да съвпада значително с него, тъй като реалното съдържание го потвърждава):

| Концепт | Категория (§10) | Въведен | Преразгледан |
|---|---|---|---|
| Ситуация | CoreModel | Седмица 3 | 6, 8 |
| Автоматична мисъл | CoreModel / BeliefHierarchy | Седмица 3 | 6, 8 |
| Емоция | CoreModel | Седмица 3 | 6, 8 |
| Телесна реакция | CoreModel | Седмица 3 | *(изисква source потвърждение — виж §22)* |
| Поведение | CoreModel | Седмица 3 | *(изисква source потвърждение — виж §22)* |
| Междинно вярване | BeliefHierarchy | Седмица 3 | 11 |
| Основно вярване / схема | BeliefHierarchy | Седмица 3 | 12 |
| Когнитивен модел (веригата) | CoreModel | Седмица 3 | 6, 8 |
| Терапевтичен алианс | TherapeuticRelationship | Седмица 5 | 6 |
| Обратна връзка | TherapeuticRelationship / SessionProcess | Седмица 5 | 6 |
| Дневен ред (agenda) | SessionProcess | Седмица 6 | — |
| Проверка на настроението | SessionProcess | Седмица 6 | — |
| Сократически въпроси | TherapeuticTechnique | Седмица 10 | — |
| Поведенческа активация | TherapeuticTechnique | *`Upcoming`* (Седмица 7, `Route=null`) | — |
| Когнитивни изкривявания | CognitiveDistortion | *`Upcoming`* (Седмица 9, `Route=null`) | — |

Списъкът **не е окончателна, заключена таксономия** — е стартова, source-confirmed основа за Phase 1 (§24); разширява се само с curriculum/source потвърждение, никога по общо познание (§40 от заявката).

**`Upcoming` правило (релаксация на v1's строго „не показвай непреподадено"):** на **глобалната** Knowledge Map, концепт от нерутирана седмица (`Route == null`, напр. "Поведенческа активация", "Когнитивни изкривявания") **може** да се показва — приглушено, без детайлна дефиниция, без advanced relationships — само като orientation landmark ("курсът отива натам"). Weekly Mind Map и Case Conceptualization Map **запазват** строгото v1 правило без изключение (§9) — там forward leak е забранен изцяло.

**Rendering:** `ConceptMapModel` (§6) → `ConceptGraph.razor` в network layout (multi-parent, labeled edges), не tree.

---

## 4. Weekly Mind Map standard

**Корекция, най-важната в тази ревизия:** v1 предлагаше nodes = "секции на седмицата" — прекалено близо до визуален Table of Contents. Поправено.

**Nodes са KNOWLEDGE CLUSTERS/CONCEPTS.** За Седмица 6 (реален пример, не хипотетичен — извлечен директно от вече съществуващото съдържание, не измислен за случая):

- Цели на първата сесия
- Задаване на дневния ред
- Проверка на настроението
- Придобиване на актуализация
- Дискусия за диагнозата
- Критерии за отклонение от структурата
- Обучение за когнитивния модел (проблеми → цели)
- Домашна работа
- Обратна връзка

Section anchors (`#6.2`, `#6.5`, `#6.7`) са **navigation metadata**, прикачени към съответния cluster node (`ConceptNode.AnchorRoute` или локален week-scoped еквивалент) — не самият node, не заместител на знанието.

**Preview и Review са ДВЕ СЪСТОЯНИЯ на една и съща knowledge структура**, не два различни модела/компонента:

| | Preview (начало на седмицата) | Review (край на седмицата) |
|---|---|---|
| Nodes | Същите knowledge clusters | Същите knowledge clusters |
| Relationships | Основни, без label detail | Пълни, с relationship labels |
| Cross-week връзки | Само prerequisite (кое трябва да сте видели) | Пълни — `BuildsOnWeekNumbers`-извлечени back-links |
| Допълнително | "какво предстои" маркер | Retrieval prompts (§13), линкове към секции |

Технически: `MindMapModel` инстанцията е една и съща; `Mode` параметър (`Preview`/`Review`) на `ConceptGraph.razor` контролира кои полета на всеки node се рендерират — не два отделни data set-а.

---

## 5. Mind Map semantic model

За hierarchy/single-parent tree — orientation и memory структура (Course Map и Weekly Mind Map):

```
MindMapNode
  Id              : string
  Label           : string
  ParentId        : string?      // null само за root
  ShortDefinition : string?
  Anchor          : string?      // route/anchor за навигация, не самото знание
  State           : ConceptState // Upcoming / Introduced / Revisited, §12
```

Строго single-parent — ако dependency-те на едно парче знание реално имат повече от един родител, това е сигнал, че то принадлежи на `ConceptMapModel` (§6), не тук.

---

## 6. Concept Map semantic model

За relationships/multi-parent network — cross-links между концепти:

```
ConceptNode
  Id              : string
  Label           : string
  Definition      : string
  IntroducedWeek  : int
  RevisitedWeeks  : IReadOnlyList<int>
  Anchor          : string?
  Category        : ConceptCategory   // §10
  State           : ConceptState      // derived, §12

ConceptRelation
  FromId          : string
  ToId             : string
  RelationType    : RelationType      // §11, controlled vocabulary
  RelationLabel   : string            // human-readable, source-grounded формулировка
  Direction       : Directed | Bidirectional
```

Пълният field-по-field каталог с обосновка е в §10.

---

## 7. Case Conceptualization semantic model

**Не** generic Node/Edge — domain-specific модел, отразяващ реалната структура на клиничната концептуализация (самият източник реферира именно тази структура като "Cognitive Conceptualization Diagram", вечеflag-нато в blueprint v1.1 на Седмица 6 като бъдещ `SourceArtifact` кандидат — Case Conceptualization Map е рендерираната, learner-facing версия на същата идея, приложена към фиктивни персонажи, не pixel-copy на книжната фигура):

```
CaseCharacter
  Id                : string
  Name              : string        // "Ирина"
  Level             : CaseLevel     // Basic / Intermediate / Challenging
  FirstAppearedWeek : int

CaseObservation
  CaseId            : string
  WeekNumber        : int
  Situation         : string?
  Thought           : string?
  Emotion           : string?
  Body              : string?
  Behavior          : string?
  Distortion        : string?
  IntermediateBelief: string?
  CoreBelief        : string?
  InterventionLink  : string?       // линк към конкретна ConceptNode/техника, приложена тук
```

Всички полета опционални — попълват се **само** когато конкретна седмица реално използва персонажа органично (наследено буквално от v1/blueprint — "future reuse only where pedagogically natural, no invented future history"). Ирина остава единственият одобрен pilot; никаква нова история не се измисля в тази ревизия.

---

## 8. ConceptGraph rendering architecture

**Renderer vs domain models — ключовото разграничение на тази ревизия.**

`ConceptGraph.razor` отговаря **само** за:
- layout mode (tree / network / blanked-retrieval);
- connectors (SVG path, същата техника като `.decision-branch`);
- accessible structure (генерирана от същите данни, не ръчно дублирана — §19);
- interaction hooks (focus/expand, не domain logic);
- responsive behavior (§20).

`ConceptGraph.razor` **не знае**: какво е автоматична мисъл, какво е core belief, кой е Ирина, какво значи Седмица 6. Domain meaning живее изцяло в `MindMapModel`/`ConceptMapModel`/`CaseConceptualizationModel` (§5/§6/§7) и в catalog-ите (`Curriculum/ConceptGraph.cs`, `Curriculum/CaseCatalog.cs`).

**Adapter слой** превежда всеки domain модел към една малка, чисто презентационна структура, която renderer-ът реално консумира:

```
GraphRenderNode(Id, Label, ParentOrSourceIds, DisplayState, Anchor)
GraphRenderEdge(FromId, ToId, Label?, Direction)
```

```
MindMapModel               →  MindMapAdapter               →  GraphRenderNode/Edge  →  ConceptGraph.razor
ConceptMapModel             →  ConceptMapAdapter             →  GraphRenderNode/Edge  →  ConceptGraph.razor
CaseConceptualizationModel  →  CaseConceptualizationAdapter  →  GraphRenderNode/Edge  →  ConceptGraph.razor
CourseWeekDefinition/Module →  CourseMapAdapter              →  GraphRenderNode/Edge  →  ConceptGraph.razor
```

`GraphRenderNode`/`GraphRenderEdge` **не са** домейн истината — те са derived, presentation-only изход на adapter-а, регенериран при всеки render, никога съхраняван като отделен source of truth. Домейн моделите (§5/§6/§7) и catalog-ите остават единствената истина за съдържание, тестове, търсене, cross-links.

---

## 9. Map relationships and cognitive navigation

Петте карти, ясно разграничени (виж epigraph-а по-горе) — тук: как learner-ът реално преминава между тях, като **cognitive navigation**, не само site navigation:

```
Course Map (къде съм в курса)
   → избира седмица
   → Weekly Mind Map, Preview (каква е структурата на знанието тук)
      → следва cluster node
      → теория/секция в страницата
   → Weekly Mind Map, Review (мога ли да я възстановя)
      → concept, изискващ по-дълбока връзка
      → CBT Knowledge Map (как се свързва с останалото, което знам)
         → concept node от друга седмица
         → link обратно към Weekly Mind Map на тази седмица
      → Case Conceptualization Map (как изглежда приложено към Ирина)
         → connection обратно към теорията, обяснила го
```

Progressive-disclosure границата **не е еднаква за всички пет**:

| Карта | Смее ли да показва непреподадено? |
|---|---|
| Course Map | Да — целият списък от 15 седмици е винаги видим (вече установена практика, честни статус етикети) |
| CBT Knowledge Map (глобална) | Да, само като `Upcoming` — приглушено, без дефиниция/relationships (§3) |
| Weekly Mind Map | Не — само вече преподадено в тази и по-ранни routed седмици |
| Concept Map (в контекста на седмица) | Не — същото ограничение като Weekly Mind Map |
| Case Conceptualization Map | Не — най-строго, само observations от вече routed седмици |

---

## 10. Concept metadata catalog

Финален field-по-field договор за `ConceptNode` (изгражда §6, с обосновка на всяко поле):

| Поле | Тип | Защо съществува |
|---|---|---|
| `Id` | string | Стабилен идентификатор за cross-links/tests |
| `Label` | string (BG) | Основен показван термин |
| `EnglishLabel` | string? | Само където терминът реално го изисква (напр. "cognitive triad" в академичен контекст) — не за всеки |
| `ShortDefinition` | string | Reveal panel съдържание при node focus |
| `IntroducedWeek` | int | Единствената седмица, в която концептът се въвежда за пръв път |
| `RevisitedWeeks` | IReadOnlyList\<int\> | Кои по-късни routed седмици реално го преразглеждат — захранва Review-mode cross-links (§4) |
| `PrerequisiteConceptIds` | IReadOnlyList\<string\> | Concept-level зависимост (различно от Course Map's `PrerequisiteWeekNumber`, §2) |
| `RelatedConceptIds` | IReadOnlyList\<string\> | Non-hierarchical cross-links за Concept Map (§6) |
| `AnchorRoute` | string? | Route + anchor към конкретната секция, обучаваща концепта |
| `Category` | `ConceptCategory` | Групиране за навигация/филтриране (§3 таблица) — илюстративен starting set, не окончателно изчерпателен |
| `SourceReference` | string | SRC-ID + локатор (глава/фигура), traceability-принципа от DoD непроменен |

Никое поле не е добавено без реална функция (изрично изискване на заявката, §34) — всяко поле по-горе се използва от поне една конкретна секция от тази архитектура.

**Разграничение, важно да не се сгреши:** `ConceptNode.RevisitedWeeks` (концепт-ниво: кои седмици преразглеждат ТОЗИ концепт) е различно поле от `CourseWeekDefinition.BuildsOnWeekNumbers` (седмица-ниво: коя по-ранна седмица ТАЗИ седмица съзнателно надгражда) — двете обслужват различни карти (Knowledge Map срещу Weekly Mind Map cross-links) и не се сливат в едно поле, за разлика от v1's единствено `ReinforcesWeekNumbers`.

---

## 11. Relationship metadata model

Минимален controlled vocabulary, изведен от реално прочетено съдържание (не измислен предварително — всеки ред по-долу има конкретен източник от вече прочетения код/blueprint):

| RelationType | BG label | Реален пример | Direction |
|---|---|---|---|
| `LeadsTo` | „поражда" | Ситуация → Автоматична мисъл → Реакция (Седмица 6.5) | Directed |
| `DiffersFrom` | „разграничава се от" | "Актуализация" срещу "Проверка на настроението" (Седмица 6.8, Case Lab, Мартин — изрично маркирана "честа грешка") | Bidirectional |
| `IsPartOf` | „част е от" | "Проверка на настроението" е част от "Начало на сесията" | Directed |
| `Precedes` | „предхожда" | U30/U33 forward-references към бъдещи седмици (Седмица 6 blueprint §3) | Directed |
| `Supports` | „укрепва" | Обратна връзка укрепва Терапевтичния алианс (Седмица 6.7) | Directed |
| `IsExampleOf` | „пример е на" | "Обяд с колеги" сценарият е пример на когнитивния модел | Directed |

Списъкът се разширява **само** при ново, source-confirmed отношение — не предварително, изчерпателно проектиран taxonomy (§35 от заявката, спазено буквално).

---

## 12. Curriculum-state model

**Премахнати изцяло:** `Locked`/`Available`/`Reinforced` (v1) — създаваха погрешно усещане, че платформата знае learner progress. Тя не знае — няма persistent progress engine (ADR-002/ADR-003 непроменени).

**Нов `ConceptState` enum**, приложим **само** върху `ConceptNode` (не върху седмици — `CourseWeekStatus` на седмиците остава изцяло непроменен, отделен, вече съществуващ):

- **`Upcoming`** — концептът съществува в catalog-а, но `IntroducedWeek`'s `CourseWeekDefinition.Route == null` (не е рутирано/преподадено още).
- **`Introduced`** — `IntroducedWeek` е рутирано (`Route != null`); концептът се преподава.
- **`Revisited`** — поне една седмица от `RevisitedWeeks` също е рутирана — концептът е въведен по-рано И настоящо рутиран curriculum реално го използва/преразглежда отново.

Derivation function, аналогична на вече съществуващия `CurriculumLabels.DeriveStatus()`:

```
ConceptStateResolver.Derive(ConceptNode, CourseCatalog) → ConceptState
```

**Никога:** `Mastered`, `Learned`, `Completed`, "Reinforced by learner" — никое състояние не твърди нищо за конкретния learner; всичко е curriculum-ниво метаданно, изведено единствено от кои седмици са рутирани, точно както `DeriveStatus()` вече прави за седмиците.

---

## 13. Retrieval Practice architecture

Формален статус: project-wide standard, не опция. Четирите механизма от v1, потвърдени и разширени:

1. **Attempt-before-reveal флашкарта** — copy промяна на `.progressive-explanation`: "Опитай да си спомниш определението, преди да разгънеш."
2. **Reconstruct-the-process** — Level B "Ordering" от `ScenarioSimulator` (вече построена механика), извадена за самостоятелно преизползване извън симулатора.
3. **Label-the-blank-map** — `MindMapModel`/`ConceptMapModel` в `BlankedRetrieval` режим (labels скрити, learner попълва от word bank преди reveal) — режим-флаг на `ConceptGraph.razor`, не нов компонент.
4. **Explain-before-reveal** — свързва се директно с Guided Discovery (§15).

Плюс, изрично добавени по искане на собственика: **predict-before-explanation** (вече принцип в v1's §15 Guided Discovery, тук формално причислен и към retrieval семейството) и **concept matching** (Level A "Matching" от `ScenarioSimulator" — вече съществуващ механизъм, преброен коректно тук като retrieval-adjacent, не чисто recognition — виж §14 за точната граница).

---

## 14. Recognition vs Retrieval vs Application vs Reasoning taxonomy

Разграничение, позволяващо реален audit на learning depth — не всеки cluster се нуждае от четирите, но решението за всеки трябва да е съзнателно (§17).

| Категория | Определение | Пример от реално съществуващ код |
|---|---|---|
| **Recognition** | Learner избира верен отговор сред опции/разпознава готово обяснение | Проверки 1–4/5–9 (Седмица 6.2/6.5) — MC `<details>` reveal; `ScenarioSimulator` Level A "Identify-the-step" |
| **Retrieval** | Learner формулира/възстановява/подрежда ПРЕДИ да види отговора | `ScenarioSimulator` Level B "Ordering" (реален пример — learner подрежда 4 стъпки от паметта, keyboard up/down, не drag) |
| **Application** | Learner прилага concept върху нов сценарий/казус | Case Lab (Мартин/Ирина/Радо, Седмица 6.8); `ScenarioSimulator` Level B "Choose-next-step" |
| **Reasoning** | Learner анализира/сравнява/решава/обосновава разклонение | `ScenarioSimulator` Level C branching (7-node state machine); Проверка 7 ("идентифицирай грешката") |

**Важно уточнение (`<details>` не е retrieval сам по себе си):** `.progressive-explanation`/`WhatIfBox` accordions в текущия Week 6 v2 код са honest примери за **explanation/feedback**, не retrieval — освен ако не предхожда изрична recall/reconstruct подкана. Няма нужда да се преработват сега (Week 6 остава недокоснат) — но бъдещият Retrieval Coverage audit (§17) трябва да ги брои коректно, не автоматично като "retrieval, защото е `<details>`".

**Илюстративен, реален (не измислен) coverage одит на Седмица 6** — демонстрира механизма, не изисква промяна в кода сега:

| Cluster | Recognition | Retrieval | Application | Reasoning |
|---|---|---|---|---|
| 6.2 Начало на сесията (agenda/mood/update/diagnosis) | ДА (Проверки 1–4) | НЕ | НЕ | Частично (Проверка 2 близо до reasoning, но остава MC) |
| 6.3 Терминология (11 термина) | Частично (пасивно разгъване) | НЕ | НЕ | НЕ |
| 6.5 Beck in Practice (проблеми→цели, когнитивен модел) | ДА (Проверки 5–9) | НЕ | Частично (Проверка 6) | Частично (Проверка 7) |
| 6.8 Case Lab | НЕ | НЕ | ДА (Мартин/Ирина/Радо) | Частично |
| 6.9 Simulator | ДА (Level A) | **ДА** (Level B ordering) | ДА (Level B/C) | **ДА** (Level C) |

Заключение от одита: retrieval **не е системно нула** (симулаторът вече го доказва), но е **систематично отсъстващо** точно от теоретичните секции (6.2/6.3/6.5) — прецизно потвърждава собственическата диагноза, без да е нужна нова хипотеза.

---

## 15. Guided Discovery integration

Непроменено от v1 по същество, само линкнато към новата таксономия: Observe → Question → Learner response → Reveal reasoning → Link към concept/knowledge map node. Механизмът вече е валидиран (`WhatIfBox`, `SocraticDialogueExplorer`) — прилага се селективно, никога механично на всяка секция (Седмица 6 го използва 5 от 14 пъти — прецедентът остава ориентир за бъдещи седмици, не правило за 100% покритие).

---

## 16. Cognitive Representation Coverage

DoD gate A (заменя v1's версия — премахнати механичните количества):

1. Всеки major knowledge cluster е изрично оценен за най-подходящата представяне (§3 от v1 таксономията, непроменена) — документирано решение, не подразбирано.
2. Ключовите отношения (не йерархия) имат Concept Map поддръжка.
3. Ключовата йерархия има Mind Map поддръжка — Preview + Review състояние (§4).
4. Последователните процеси са Process Diagrams.
5. Условната логика е Decision Tree.
6. Major разграничения имат Comparison Matrix.
7. **Memory Anchor се използва само когато има ясна mnemonic function** — без задължителен минимум или максимум; 0, 1 или 2 могат да бъдат напълно правилни според съдържанието. (Премахнато: v1's механично "maximum ~1 на седмица".)
8. Визуалите остават source-grounded.
9. Нула декоративен визуален шум — всеки визуал проследим до конкретна учебна функция.
10. Accessibility fallback присъства за всеки graph/tree/network визуал, генериран от същите данни (§19).

---

## 17. Retrieval Practice Coverage

Нов DoD gate B:

> Всеки major knowledge cluster е оценен за подходящ retrieval method. Модулът съдържа реални recall/reconstruction възможности, а не само recognition и reveal.

Не е нужно всеки cluster да има собствено упражнение — но решението трябва да е **съзнателно, документирано** (същия одит формат като илюстрацията в §14: | Cluster | Recognition | Retrieval | Application | Reasoning |). Retrieval **не трябва системно да е нула** през целия модул — точно диагнозата, потвърдена от Седмица 6 одита по-горе (нула в теоретичните секции, но не нула цялостно благодарение на симулатора).

---

## 18. Updated Deep Learning DoD v3

Пълен текст на новата точка 8 от `06_QA_STRATEGY.md` → "Deep Learning Week — Definition of Done", **готов за прилагане в бъдеща сесия** (не приложен сега, §41 от заявката спазено — 06 файлът не е пипнат в тази ревизия):

> **8а. Cognitive Representation Coverage** — виж §16 по-горе, пълния 10-точков checklist.
> **8б. Retrieval Practice Coverage** — виж §17 по-горе.

Точки 1–7 и 9–22 от съществуващия DoD v2 (`06_QA_STRATEGY.md`) остават **напълно непроменени**.

---

## 19. Accessibility architecture

Запазено и усилено спрямо v1:

- Пълен семантичен fallback (`<details><summary>Текстово описание на диаграмата</summary>` + вложен `<ul>`/`<ol>`/`<dl>`) — вече доказан механизъм от `.decision-branch`.
- Keyboard навигация през естествения tab order на fallback списъка; никога drag за нещо задължително.
- Никаква връзка, кодирана само чрез цвят — текстов label на всяка връзка/branch.
- Screen reader резюме — едно изречение преди диаграмата.

**Ново, изрично изискване на тази ревизия:** graph визуалът и текстовият fallback **произлизат от едно и също извикване на adapter-а** (§8) — `GraphRenderNode`/`GraphRenderEdge` захранва едновременно SVG рендера И fallback списъка в един проход, не два ръчно поддържани съдържателни варианта. Това гарантира, че visual и text никога не се разминават с течение на времето — структурна гаранция, не дисциплинарно правило за автора.

---

## 20. Responsive/mobile architecture

Три, не два, изрично различни rendering strategies (desktop и mobile бяха в v1; tablet добавен тук):

| Устройство | Стратегия |
|---|---|
| **Desktop** | Пълен graph/tree overview, всички nodes и connectors видими наведнъж |
| **Tablet** | Compact graph — focused branch view (един под-клон разгънат, останалите свити), не пълно смаляване на desktop layout-а |
| **Mobile** | Expandable hierarchy / focus-one-node режим / relationship list (текстов, не графичен) |

Desktop graph никога не се "shrink"-ва буквално към малък екран — всеки breakpoint получава архитектурно различна презентация на същите данни (същия принцип, вече доказан от `.guided-practice-sequence`'s wide/narrow режими).

---

## 21. SSR + progressive WASM strategy

**Static SSR по подразбиране** за всичките пет карти (Course Map, CBT Knowledge Map, Weekly Mind Map, Concept Map, Case Conceptualization Map) — видими, четими, navigable, семантично пълни без WASM hydration. Node-focus interaction (виж определение/линкове) реализирано през native `<details>`/CSS, не JS.

**WASM само като progressive enhancement**, и само където има реална learning interaction:

- Blank-map retrieval (§13, label-the-blank-map режим).
- Filtering/dynamic focus (напр. "покажи само тази категория концепти").
- Active node reconstruction упражнения.
- По-богато interactive state, реално изискващо client-side логика отвъд disclosure.

Не се превръщат всички карти в Interactive WebAssembly по подразбиране — SSR е основата, WASM е добавка само за конкретен, обоснован interaction, точно както вече установената практика на проекта (Static SSR за информационно съдържание, WASM само за упражнението/private-data компоненти, `20_TECHNOLOGY_DECISION.md`).

**No-graph-library решение — потвърдено, но преформулирано като текущо, не вечно:**

> **CURRENT IMPLEMENTATION DECISION:** без D3/Cytoscape/vis-network/Mermaid на този етап — native SVG/CSS вече доказа достатъчност (`.decision-branch`, `.concept-map__flow`) за реалния мащаб (15 week-nodes + шепа concept nodes на седмица).
>
> **Revisit trigger:** ако native SVG/CSS layout стане прекомерно сложен, труден за поддръжка, inaccessible, или недостатъчен за реалния graph размер (напр. Knowledge Map-ът реално нарасне до десетки силно кръстосани concept relations), решението се преоценява explicit — не се приема мълчаливо като постоянна забрана.

---

## 22. Week 6 reference implementation gap analysis

**Не се пипа сега** — само актуализирана карта за бъдещ, отделен implementation prompt (Phase 3, §24):

| Pattern | Текущо състояние | Промяна, нужна за v1.1 съответствие |
|---|---|---|
| Weekly Mind Map (Preview) | Проза (6.0) | Рендериран `MindMapModel` инстанция, cluster nodes (§4), не section list |
| Weekly Mind Map (Review) | Проза bullet list (6.12) | Същия компонент, `Mode=Review`, с реални cross-week релации |
| Concept Map (С→М→Р) | Линейна верига, 3 nodes | Разширение до пълен С→М→Е→Тяло→Поведение — **изисква source/curriculum потвърждение** дали Глава 5 самата описва отделни Emotion/Body/Behavior nodes на тази точка от разказа, или дали "Реакция" остава честно комбиниран label в оригиналния first-session пример — маркирано `Requires source/curriculum confirmation`, не приема се предварително |
| Case Conceptualization Map (Ирина) | Чист текст в Case Lab card | Рендерирана верига веднъж щом `CaseConceptualizationModel`/`ConceptGraph` съществуват |
| Retrieval Practice | Виж §14 одита — силно в симулатора, отсъства в теорията | Reconstruct-widget (извлечен от Level B ordering) приложен и извън симулатора |
| Teach-Back | Липсва | Directно добавимо като prompt преди `WhatIfBox` reveal |
| Cumulative links | Проза само ("виж Седмица 3") | Метаданно-управлявано, щом `BuildsOnWeekNumbers` съществува |

**Всичко останало в Седмица 6 v2 вече съответства на целевата архитектура** — decision tree (V4), memory anchor (V3), source artifacts (V5/V6) не изискват промяна.

---

## 23. Final component/data architecture

Консолидирана таблица — всичко предложено в тази ревизия, минимална повърхност:

| Файл/тип | Роля |
|---|---|
| `Curriculum/ConceptGraph.cs` | `ConceptNode`, `ConceptRelation`, `ConceptCategory`, `RelationType`, `ConceptState` — Knowledge Map domain модел (§6/§10/§11/§12) |
| `Curriculum/CaseCatalog.cs` | `CaseCharacter`, `CaseObservation` — Case Conceptualization domain модел (§7) |
| `MindMapModel` (в код, не отделен catalog файл) | Weekly Mind Map / Course Map tree структура (§5) |
| `Components/Shared/ConceptGraph.razor` | Единствен generic renderer — tree/network/blanked-retrieval режими (§8) |
| `MindMapAdapter` / `ConceptMapAdapter` / `CaseConceptualizationAdapter` / `CourseMapAdapter` | Domain модел → `GraphRenderNode`/`GraphRenderEdge` (§8) |
| `.concept-graph` CSS extension | Extended `.decision-branch`/`.concept-map__flow` техника — SVG connector, N-ниво/network layout |
| `CourseWeekDefinition.PrerequisiteWeekNumber` (ново поле) | Course Map навигационно подредба (§2) |
| `CourseWeekDefinition.BuildsOnWeekNumbers` (ново поле, заменя v1's `ReinforcesWeekNumbers`) | Knowledge Map cross-link генерация (§10) |

Нищо извън тази таблица не се предлага — умишлено минимална component повърхност (§29 от v1, непроменен принцип).

---

## 24. Revised implementation sequence

Собственически одобрен ред (§30 от заявката), **никоя фаза не е изпълнена в тази ревизия**:

- **Phase 1 — Semantic models/contracts.** `ConceptNode`/`ConceptRelation`/`MindMapNode`/`CaseCharacter`/`CaseObservation` + двете нови `CourseWeekDefinition` полета. Без UI.
- **Phase 2 — `ConceptGraph` rendering engine + accessibility.** Tree/network/blanked-retrieval режими, adapter слой, fallback-от-същите-данни гаранция (§19).
- **Phase 3 — Week 6 reference implementation.** Weekly Preview Mind Map; Weekly Review Mind Map; пълен Concept Map (с source-confirmation стъпка за С-М-Е-Тяло-Поведение разширението); Case Conceptualization Map за Ирина; retrieval enhancements (reconstruct widget извън симулатора, teach-back prompts).
- **Phase 4 — OWNER VISUAL + LEARNING REVIEW.** Гейт, не автоматичен преход — идентичен модел на съществуващия Deep Learning DoD's последна точка.
- **Phase 5 — Course Map + CBT Knowledge Map on `/kurs/karta`.** Двата режима (§2/§3), само след Phase 4 одобрение.
- **Phase 6 — Retrofit на вече routed седмици (1/3/8/10/12), само където е уместно.** Source-confirmed links само (§31) — не механично прилагане навсякъде.
- **Phase 7 — Възобновяване на Week 7 build order.** Замразен до тук.

Причина за реда (собственическа, потвърдена): Седмица 6 вече има 47-unit source coverage, дълбочина, cases, симулатор, визуална основа — тя е най-добрата лаборатория за reference implementation преди global rollout.

---

## 25. Closed owner decisions

- **`/kurs/karta`:** ДА — постоянна функция на course-level навигация. Минимум: видим достъп от `/kurs` hub; back-link от всяка weekly Review Map; постоянен course-level nav entry. Не е задължително в тази фаза да влиза в най-глобалната sidebar навигация на целия сайт (IA непроменена сега).
- **SSR/WASM:** Static SSR по подразбиране за всичките пет карти; WASM само като progressive enhancement за реална interaction (§21).
- **Build order:** Week 6 reference implementation преди global rollout; Week 7 остава frozen до Phase 7 (§24).
- **Retroactive metadata:** ДА, позволено върху вече routed седмици (1/3/6/8/10/12) — но **не се попълва сега на сляпо**; само след Week 6 reference approval, само source/curriculum-confirmed връзки, никакви invented relationships.

---

## 26. Remaining unresolved owner decisions

`None.`

(Единствен нестопиращ имплементационен детайл, не собственическо решение: дали `/kurs/karta`'s два режима се показват като tabs или единен view с toggle — чиста UX преценка за Phase 5, не архитектурен blocker.)

---

## 27. Recommendation

**READY FOR IMPLEMENTATION PROMPT: YES.**

Course structure и knowledge structure са архитектурно разделени (§2/§3), не просто декларативно. Три отделни domain модела заменят единствения generic Node/Edge риск (§5/§6/§7), свързани към общ renderer през adapter слой, който пази domain semantics извън presentation layer-а (§8). Retrieval вече е разграничено от recognition с проверим audit механизъм (§14/§17), не просто принципна декларация. Curriculum-state semantics вече не намекват за learner progress, който платформата няма (§12). Всички механични количества (memory anchor максимум, единственото "минимум 3 визуализации" правило) са премахнати в полза на съзнателна, документирана преценка. Собственическите решения от заявката са затворени изрично (§25); не остават реални blockers (§26). Седмица 6/7/12 остават недокоснати; никакво ново curriculum съдържание не е измислено; всяко несигурно cross-reference е маркирано `Requires source/curriculum confirmation`, не запълнено от общо познание.

---

Целта остава непроменена от v1: learner-ът да не помни "това беше в Седмица 8", а постепенно да изгради **вътрешен модел на КПТ** — да позиционира concept, обясни значение, види relationships, възстанови процес, различи близки concepts, приложи concept върху казус, свърже новото с вече изученото, и извлече знанието от паметта без готов отговор пред себе си.

**СПРИ.** Не е писан `.razor`/CSS/тест/`CourseCatalog.cs` код в тази сесия. Week 6, Week 7, Week 12 остават недокоснати. Не е направен git commit.
