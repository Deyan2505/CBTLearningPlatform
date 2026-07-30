# 21 — Content and Data Model

**Статус: `OWNER APPROVED WITH MVP STORAGE CONSTRAINTS`** (ADR-007, 2026-07-30). Потвърдено: Markdown (уроци/модули/ресурси/речник) + JSON (упражнения/въпроси/citations) + browser storage (прогрес). Разширение спрямо оригиналната версия по-долу: **Lesson метаданните трябва да включват и `reviewStatus` и `professionalReviewStatus`** отделно (не само общ "status"), плюс `version`. **IndexedDB се препоръчва пред `localStorage`** за по-структурирани локални данни (напр. упражнението), ако е реално необходимо; `localStorage` остава само за малки, нечувствителни настройки. Черновата на упражнението в browser storage се пази само след изрично потребителско действие и подходящо предупреждение, не автоматично. Потвърдено без промяна: никаква база данни, CMS, административен панел или authentication инфраструктура в MVP.

## Сравнение на съхранение

| Подход | За MVP? | Обосновка |
|---|---|---|
| Markdown файлове (front-matter + текст) | **Да, за уроци/статии** | Version-controllable, лесно за преглед в PR, не изисква инфраструктура (ADR-003) |
| JSON файлове | **Да, за структурирани данни** (изкривявания, речник, въпроси) | По-лесно за програмен достъп от чист Markdown; все така версионируемо в Git |
| Релационна база данни | Не в MVP | Излишна сложност за обема съдържание в MVP; отложено до Фаза 6 |
| Headless CMS | Не | Външна зависимост, потенциален разход, без ясна нужда за 1 автор в MVP |
| Административен панел | Не в MVP | REQ-ADMIN-001, Фаза 6 |

**Препоръка за MVP: Markdown за текстово съдържание (уроци/статии) + JSON за структурирани списъци, всичко в Git.**

## Модели

| Модел | Предназначение | Ключови полета | Задължителни | Връзки | MVP? | Съхранение | Чувствителни данни? | Кой променя | Версиониране | Валидация |
|---|---|---|---|---|---|---|---|---|---|---|
| **Course** | Най-високо ниво на групиране (в MVP може да съвпада с 1 програма) | id, title, description, order | id, title | 1→N Module | Да (минимална, вероятно 1 запис) | JSON/Markdown front-matter | Не | Собственик (Git commit) | Git история | Уникален id, title непразен |
| **Module** | Тематична група от уроци (напр. "Ситуация→Мисъл→Емоция→Поведение") | id, courseId, title, slug, order, prerequisiteModuleIds | id, courseId, title, slug | N→1 Course, 1→N Lesson | Да | JSON метаданни + папка с Markdown уроци | Не | Собственик | Git | Уникален slug в рамките на Course |
| **Lesson** | Отделен урок в модул | id, moduleId, title, slug, order, learningObjectiveIds, content (Markdown), citationIds | id, moduleId, title, slug, content | N→1 Module, N→N LearningObjective, N→N Citation | Да | Markdown файл с front-matter | Не | Собственик, бъдещ редактор (Фаза 6) | Git | Content non-empty, поне 1 citation ако съдържа клинично твърдение |
| **LessonSection** | Под-секция вътре в урок (пример, обобщение и т.н.) | id, lessonId, type (intro/example/summary/check), order, content | id, lessonId, type, content | N→1 Lesson | Опция — може да е просто Markdown heading в MVP, не отделен модел | Част от Lesson.content | Не | Собственик | Git (част от Lesson файла) | N/A ако вградено в Lesson |
| **Exercise** | Интерактивно упражнение (напр. thought record) | id, lessonId, type, fields (schema), instructions | id, lessonId, type, fields | N→1 Lesson, 1→N Question (ако е тип quiz) | Да (1 упражнение в MVP — thought record) | JSON schema за полетата; UI компонент в кода | Не (самата дефиниция); **Да** за потребителски отговори (виж ProgressRecord) | Собственик | Git | fields non-empty, type от затворен списък |
| **Question** | Въпрос в "проверка на разбирането" | id, lessonId, text, type (single/multi/reflective) | id, lessonId, text, type | N→1 Lesson, 1→N AnswerOption (ако не е reflective) | Да (рефлективни въпроси в MVP; без точкуване) | JSON или front-matter в Lesson | Не | Собственик | Git | text непразен |
| **AnswerOption** | Опция за отговор (само за не-рефлективни въпроси, ако изобщо се въведат в MVP) | id, questionId, text, isHelpfulReflection | id, questionId, text | N→1 Question | Не в MVP (MVP използва чисто рефлективни въпроси без "верен/грешен" отговор — REQ-FUNC-003) | JSON | Не | Собственик | Git | N/A за MVP |
| **Citation** | Библиографска препратка към източник | id, sourceRef (SRC-XXX), locator (глава/фигура/стр.), fullReference | id, sourceRef, fullReference | N→N Lesson | Да (принципно — REQ-CONT-004) | JSON, централен citations.json | Не | Собственик | Git | sourceRef валиден SRC-ID от `11_SOURCE_REGISTER.md` |
| **GlossaryTerm** | Речников термин | id, term, definition, relatedLessonIds | id, term, definition | N→N Lesson (опционално) | Да (базов, 5–10 термина) | JSON | Не | Собственик | Git | term уникален |
| **LearningObjective** | Резултат от обучение за урок/модул | id, text, moduleId или lessonId | id, text | N→1 Module/Lesson | Опция — може да е поле в Lesson вместо отделен модел в MVP | Част от Lesson front-matter | Не | Собственик | Git | N/A ако вградено |
| **ProgressRecord** | Кой урок/упражнение е завършен от текущия потребител | lessonId или exerciseId, completedAt, exerciseData (ако е упражнение) | lessonId, completedAt | Логическа връзка към Lesson/Exercise (не FK — клиентски модел) | Да | **Browser localStorage** (не сървър — ADR-002) | **Да — exerciseData може да съдържа лични мисли/ситуации от thought record** | Самият потребител (клиентски) | Няма версиониране — презаписва се локално | Валидация в клиентския код преди запис |
| **ContentReview** | Запис за клиничен/редакторски преглед на съдържателна единица | id, lessonId, reviewerRole, status, date, notes | id, lessonId, status | N→1 Lesson | Не като код/UI в MVP — **процесно да, чрез `07_CONTENT_GOVERNANCE.md`** (напр. таблица/чеклист, не база данни) | Markdown/таблица в проектната документация | Не | Собственик + бъдещ рецензент | Git (история на прегледите) | Всеки Lesson публикуван без "reviewed" статус блокира publish (процесно правило) |
| **ContentVersion** | История на промените в съдържателна единица | (имплицитно чрез Git commit история) | N/A — не отделен модел | N/A | Да, но **чрез Git, не отделна таблица** (ADR-003) | Git история | Не | Git | Git е самото версиониране | N/A |

## Ключово архитектурно решение

**ProgressRecord е единственият модел с потенциално чувствителни данни в MVP**, и той **никога не напуска браузъра на потребителя** (local storage, ADR-002). Няма сървърна база данни, следователно няма сървърно съхранени лични данни за изтичане или неправомерен достъп в MVP архитектурата.

`ContentReview` и `ContentVersion` не са технически модели/таблици в MVP — те са **процесни** артефакти (документация, Git история), съзнателно не дигитализирани в отделна система преди да съществува реален обем съдържание и повече от един автор/редактор (виж REQ-ADMIN-002/003, Deferred до Фаза 6).

## Отворени решения

- Точната JSON схема на `Exercise.fields` за thought record упражнението — да се дефинира в началото на Фаза 4 (техническа имплементация), не в тази документална фаза.
- Дали `LessonSection` и `LearningObjective` заслужават отделни файлове/модели или остават вградени в Lesson Markdown front-matter — препоръка: вградени за MVP простота, преразглеждане при обем >20 урока.
