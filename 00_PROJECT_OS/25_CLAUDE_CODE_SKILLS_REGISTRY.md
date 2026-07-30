# 25 — Claude Code Skills Registry

Пълен инвентар на Claude Code skills/commands/agents/rules/hooks/MCP, извършен 2026-07-30 преди Phase 1 техническа работа. Всеки запис е реално проверен (файл/конфигурация отворени), не предположен по име.

## Обхват на търсенето

| Ниво | Проверено местоположение | Резултат |
|---|---|---|
| `PROJECT` | `Когнитивно-Поведенческа Терапия/CLAUDE.md`, `Когнитивно-Поведенческа Терапия/.claude/` | **Не съществуват** |
| `PARENT` | `ПРЕДИЗВИКАТЕЛСТВА/CLAUDE.md`, `ПРЕДИЗВИКАТЕЛСТВА/.claude/` | **Не съществуват** |
| `USER` | `C:\Users\deian\.claude\` (skills/commands/agents/rules/hooks подпапки), `C:\Users\deian\.claude\CLAUDE.md`, `C:\Users\deian\CLAUDE.md` | Няма отделни `skills/`, `commands/`, `agents/`, `rules/`, `hooks/` папки; няма user-level `CLAUDE.md`. Skills/agents/commands се управляват през вграден plugin/marketplace регистър (виж по-долу), не през директни файлове в тези подпапки. |
| `GLOBAL` (plugin marketplace) | `C:\Users\deian\.claude\plugins\` | 2 marketplace регистрирани (`claude-plugins-official`, `ponytail`); само **ponytail** плъгин реално инсталиран |
| `settings.json` | `C:\Users\deian\.claude\settings.json` | Няма конфигурирани `hooks` или `mcpServers` ключове; само `model` |

**Заключение на обхвата:** няма никакви project-specific или parent-specific Claude инструкции за тази платформа. Наличните skills/agents идват изцяло от глобалния/plugin регистър на средата (surfaced в системния списък в началото на сесията) — не от файлове, browse-нати директно в `.claude/skills/`.

## Регистър

| ID | Име | Вид | Местоположение | Scope | Предназначение | Trigger | Подходящи фази | Ограничения | Прочетен изцяло? | Статус |
|---|---|---|---|---|---|---|---|---|---|---|
| SKILL-001 | `ponytail:ponytail` | SKILL (плъгин, **активен**, ниво `full`) | Plugin, инсталиран локално | GLOBAL | Налага най-простото работещо решение — YAGNI стъпаловидна логика (нужно ли е → има ли го вече в кода → stdlib → нативна платформена функция → вече инсталирана зависимост → 1 ред → минимален код) | Автоматичен за целия codebase, вкл. Blazor/.NET работа | Всички технически фази (1–9) | Не отменя explicit user изисквания, accessibility, security, error handling на trust boundary | Да (описан в system-reminder в началото на сесията) | `SELECTED` — активен по подразбиране за цялата техническа работа |
| SKILL-002 | `ponytail:ponytail-review` | SKILL | Plugin | GLOBAL | Преглед само за over-engineering (какво да се изтрие) | `/ponytail-review`, "review for over-engineering" | Фаза 7 (QA) и ad-hoc след всяка нетривиална стъпка | Проверява само сложност, не коректност | Да | `RELEVANT` — ще се ползва при код ревюта |
| SKILL-003 | `ponytail:ponytail-audit` | SKILL | Plugin | GLOBAL | Одит на целия repo за bloat/over-engineering | "audit this codebase" | Периодично, след няколко завършени модула | Едноразов отчет, не прилага фиксове | Да | `NOT RELEVANT` засега (няма codebase още) |
| SKILL-004 | `simplify` | SKILL | Вграден | GLOBAL | Преглед на променения код за reuse/simplification/efficiency, прилага фиксовете | След всяка съществена промяна | Всички технически фази | Само качество, не търси бъгове | Да | `RELEVANT` — ще се ползва след всеки STEP |
| SKILL-005 | `security-review` | SKILL | Вграден | GLOBAL | Пълен security преглед на pending промени | Преди merge на security-relevant код | Особено Фаза 4 (упражнение с потребителски вход), Фаза 8 (deployment) | Изисква реален diff за преглед | Да | `RELEVANT` — задължителен преди STEP, докосващ упражнението или auth |
| SKILL-006 | `review` | SKILL | Вграден | GLOBAL | Преглед на GitHub PR | Наличие на отворен PR | След като има реален Git repo + PR workflow | Изисква GitHub remote | Да | `NOT RELEVANT` засега (няма repo/PR още) |
| SKILL-007 | `run` | SKILL | Вграден | GLOBAL | Стартира и управлява проекта, за да се провери реална промяна в браузър | "run the app", проверка на UI промяна | Фаза 2+ (когато има реален UI за показване) | Търси проектен skill за стартиране първо, после built-in fallback | Да | `RELEVANT` за бъдещи UI стъпки, `NOT RELEVANT` за STEP-1.1 (скеле без UI логика отвъд шаблона) |
| SKILL-008 | `init` | SKILL | Вграден | GLOBAL | Инициализира `CLAUDE.md` с документация на codebase | При начало на реален код | Начало на Фаза 1 | Генерира нов CLAUDE.md — не съществува такъв в момента | Да | `RELEVANT` — препоръка по-долу да се извика след STEP-1.1 |
| SKILL-009 | `dataviz` | SKILL | Вграден | GLOBAL | Насоки за визуализации/графики/dashboard-и | Създаване на chart/graph | Не се очаква в MVP (платформата няма dashboard с графики) | N/A | Не (не е приложим) | `NOT RELEVANT` |
| SKILL-010 | `artifact-design` / `artifact-capabilities` | SKILL | Вграден | GLOBAL | Насоки за Claude Artifacts (публикувани самостоятелни HTML/MD страници) | Публикуване на Artifact | Не е приложимо — платформата е реално Blazor приложение, не Claude Artifact | N/A | Не | `NOT RELEVANT` |
| SKILL-011 | `claude-api` | SKILL | Вграден | GLOBAL | Референция за Claude/Anthropic API интеграции | Споменаване на Claude/LLM интеграция | Не е приложимо — AI съветник е изрично `REMOVE`/извън MVP (`14_EXISTING_PROTOTYPE_AUDIT.md`, `19_MVP_SCOPE.md`) | N/A | Не | `NOT RELEVANT` |
| SKILL-012…041 | `vercel:*` (30 skills — deploy, nextjs, shadcn, ai-sdk и др.) | SKILL | Вграден (plugin) | GLOBAL | Vercel платформа, Next.js специфични | Не се тригерват | Не е приложимо — проектът не се хоства на Vercel, не е Next.js | N/A | Не (нерелевантни по дефиниция) | `NOT RELEVANT` (всичките) |
| SKILL-042 | `update-config` | SKILL | Вграден | GLOBAL | Промяна на Claude Code `settings.json` (permissions, hooks, env vars) | Изрична заявка за такава промяна | Мета-инструмент, не проектен | N/A | Да | `NOT RELEVANT` за самата платформа; може да се ползва при нужда от нови permissions |
| SKILL-043 | `keybindings-help` | SKILL | Вграден | GLOBAL | Преконфигуриране на клавишни комбинации в Claude Code | Изрична заявка | Мета-инструмент | N/A | Да | `NOT RELEVANT` |
| SKILL-044 | `fewer-permission-prompts` | SKILL | Вграден | GLOBAL | Сканира транскрипти, добавя allowlist за по-малко permission prompts | Изрична заявка | Мета-инструмент | N/A | Да | `NOT RELEVANT` засега |
| SKILL-045 | `loop` / `schedule` | SKILL | Вграден | GLOBAL | Периодично/насрочено изпълнение на команда | Изрична заявка за recurring task | Не е приложимо за еднократна STEP-базирана работа | N/A | Да | `NOT RELEVANT` |
| AGENT-001 | `Explore` | AGENT | Вграден | GLOBAL | Бързо read-only търсене в codebase | Отваряне на голям/непознат codebase | Полезен веднага щом съществува реален Blazor solution с много файлове | Read-only | Да | `RELEVANT` за бъдещи стъпки с по-голям codebase |
| AGENT-002 | `Plan` | AGENT | Вграден | GLOBAL | Проектиране на имплементационен план | Сложни multi-file промени | Полезен за по-големи бъдещи STEP-ове (напр. дизайн система, флагмански модул) | Не пише код директно | Да | `RELEVANT`, не е нужен за STEP-1.1 (тривиален скеле) |
| AGENT-003 | `claude-code-guide` | AGENT | Вграден | GLOBAL | Въпроси за самия Claude Code/SDK/API | Въпроси "как работи Claude Code" | Мета, не проектен | N/A | Да | `NOT RELEVANT` за платформата |
| AGENT-004…006 | `vercel:ai-architect`, `vercel:deployment-expert`, `vercel:performance-optimizer` | AGENT | Вграден (plugin) | GLOBAL | Vercel-специфични | Не се тригерват | Не е приложимо | N/A | Не | `NOT RELEVANT` |
| — | Blazor / ASP.NET Core / Razor Components специализиран skill | SKILL | — | — | — | — | Фаза 1–9 (директно приложимо) | — | — | **`UNAVAILABLE`** — не съществува такъв skill в тази среда |
| — | UI/UX skill за calm/accessible mental-health образователен интерфейс | SKILL | — | — | — | — | Фаза 2 (дизайн система) | — | — | **`UNAVAILABLE`** — не съществува |
| — | Accessibility/WCAG специализиран skill | SKILL | — | — | — | — | Фаза 2, 7 | — | — | **`UNAVAILABLE`** — не съществува |
| — | .NET/xUnit/bUnit/Playwright QA skill | SKILL | — | — | — | — | Фаза 1 (тестов проект), 4, 7 | — | — | **`UNAVAILABLE`** — не съществува |
| MCP-001 | `plugin:vercel:vercel` | MCP | Plugin config | GLOBAL | Vercel API достъп | Изрична Vercel операция | Не е приложимо | Изисква OAuth оторизация, която не е налична в тази non-interactive сесия | Не е приложимо | `NOT RELEVANT` / `REQUIRES CONFIGURATION` (без значение за проекта) |

## Skill Selection Matrix

| Project Area | Candidate Skills | Selected Skill | Reason | When Used | Required Outputs |
|---|---|---|---|---|---|
| Architecture (Blazor solution setup) | Няма специализиран; `ponytail` като общ принцип | `ponytail:ponytail` (пасивно активен) | Не съществува .NET/Blazor-специфичен skill; ponytail предотвратява over-engineering в скелето | STEP-1.1 и нататък | Минимален, работещ solution без излишни абстракции |
| Blazor foundation | Няма специализиран | `NO SPECIALIZED SKILL FOUND — USING PROJECT RULES AND STANDARD ENGINEERING PRACTICES` (+ `init` след скелето) | UNAVAILABLE | STEP-1.1 | Стандартен `dotnet new blazor` изход + бъдещ `CLAUDE.md` от `init` |
| UI/UX | Няма специализиран | `NO SPECIALIZED SKILL FOUND` | UNAVAILABLE — `artifact-design` не се прилага (не е Claude Artifact) | Фаза 2 | Дизайн решения, документирани ръчно в `01_MASTER_PLAN.md` Фаза 2 и бъдещ дизайн-система документ |
| Accessibility | Няма специализиран | `NO SPECIALIZED SKILL FOUND` | UNAVAILABLE | Фаза 2, 7 | Ръчно приложение на WCAG 2.1 AA изискванията, вече документирани в `23`/`13` (REQ-A11Y-*) |
| Content model | Няма специализиран | Project rules (`21_CONTENT_AND_DATA_MODEL.md`) | Няма нужда от външен skill — моделът вече е дефиниран | Фаза 3 | Markdown/JSON файлове по вече одобрения модел |
| Privacy | Няма специализиран | Project rules (`23_CLINICAL_SAFETY_BOUNDARIES.md`, `08_DATA_PRIVACY_SECURITY.md`) | Няма нужда от външен skill | Фаза 4, винаги | Спазване на "не напуска браузъра" правилото |
| Security | `security-review` | `security-review` | Пряко приложим, вграден | Преди merge на код, докосващ упражнението/входни данни | Report на findings, без auto-fix освен ако не е поискано |
| Testing | Няма .NET-специфичен | Project rules (`06_QA_STRATEGY.md`) + стандартни xUnit/bUnit практики | UNAVAILABLE специализиран skill | Фаза 1 (тестов проект), 4, 7 | Тестове по вече дефинираните критерии |
| Documentation | `init` (за бъдещ `CLAUDE.md`) | `init`, отложен до след STEP-1.1 | Проектът все още няма код за документиране | След STEP-1.1 | `CLAUDE.md` в project root |
| Progress tracking | Project OS процесът сам по себе си | Project rules | Вече дефиниран изчерпателно | Всички фази | `02_CURRENT_STATUS.md`, `10_SESSION_LOG.md` |
| Deployment | Няма специализиран (vercel:* skills нерелевантни — не е Vercel хостинг) | Project rules (`01_MASTER_PLAN.md` Фаза 8) | UNAVAILABLE, и по дефиниция неприложим (различна платформа) | Фаза 8 | Deployment checklist, все още непопълнен |
| Post-STEP quality review | `simplify`, `ponytail:ponytail-review` | И двата, последователно | Пряко приложими, вградени | След всеки нетривиален STEP | Списък находки/приложени фиксове |

## Заключение

**Няма specialized skill за основната техническа работа на този проект** (Blazor/.NET архитектура, UI/UX за спокоен образователен интерфейс, accessibility, .NET-специфично QA). Наличните релевантни инструменти са общи (ponytail, simplify, security-review, init, run, Explore, Plan) и се прилагат като допълнение към, не заместител на, вече дефинираните Project OS правила (`13`, `19`, `20`, `21`, `23`, `24`). Всички 30 `vercel:*` skills са категорично нерелевантни — различна платформа/framework.
