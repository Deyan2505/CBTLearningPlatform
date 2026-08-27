# CBT Learning Platform — Repository Guide

This file applies to the whole repository. It is a routing and guardrail document, not a replacement
for `00_PROJECT_OS/`. Start every task by reading the owner request, `git status --short`,
`git diff`, `git diff --cached`, and `00_PROJECT_OS/02_CURRENT_STATUS.md`. The current owner request
has priority; recorded owner decisions and locked standards must not be changed implicitly.

## Repository map

- `00_PROJECT_OS/` — project governance, decisions, current status, source records, QA, and history.
- `00_PROJECT_OS/_blueprints/` — approved cognitive architecture, rollout workflow, and per-week
  source/coverage or migration plans.
- `00_PROJECT_OS/_source_corpus/` — local source extracts; gitignored and never committed.
- `CbtLearningPlatform/CbtLearningPlatform.Client/` — the entire app: a standalone Blazor WebAssembly
  project (there is no server project — it was removed when the app was converted for Netlify).
  Weekly pages are in `Components/Pages/`, shared learning/visual components in `Components/Shared/`,
  curriculum and map models in `Curriculum/`, and the design system in `wwwroot/app.css`.
- `CbtLearningPlatform/CbtLearningPlatform.Tests/` — xUnit structural, content, and regression tests.
- `CbtLearningPlatform/CbtLearningPlatform.sln` — solution entry point.
- `.github/workflows/ci.yml` — authoritative CI: restore/build/test on every push and PR, plus a
  `deploy` job (publish + push to Netlify) on push to `main`. See Deployment below.
- `kpt_syllabus.pdf` — curriculum-structure reference only, not a clinical-content source.
- `code_artifact.html` — reference prototype only, not a source of clinical truth or final architecture.

## Source-first content rules

- Never invent CBT facts, terminology, causal claims, map nodes/relations, source attributions, or case
  history. Do not infer a missing relation because it seems clinically plausible.
- For CBT content, follow the Source Lock in `11_SOURCE_REGISTER.md`: use SRC-041 with an exact
  chapter/figure/page locator. Secondary sources may cross-check or add explicit context; they do not
  override the primary source. Owner instructions are authoritative for product/process decisions.
- If a claim is not supported, stop it from entering learner-facing content. Mark draft material
  `[NEEDS-SOURCE]` or report the gap; never silently fill it from model memory.
- Preserve the educational—not diagnostic or therapeutic—boundary and follow
  `07_CONTENT_GOVERNANCE.md` and `23_CLINICAL_SAFETY_BOUNDARIES.md`.
- Fictional cases may be created only within an owner-approved/source-safe exercise scope. Never add
  unapproved history to an established longitudinal character.

## Knowledge Units and source coverage

- For a Deep Learning week, read the relevant real source in full, extract it only into the ignored
  source corpus, enumerate Knowledge Units (KUs), and map every KU to the actual final page section.
- Coverage means **100% accounted for**, not 100% included: each KU must be Included, explicitly
  Deferred to a destination, explicitly Excluded with a reason, or placed in the approved explicit
  review state. Unaccounted units must remain zero; never use the retired `>=90%` rule.
- Preserve established KU identifiers, totals, status decisions, and section mappings unless a fresh
  source audit proves a correction. Do not claim coverage from tests, summaries, or memory alone.
- Every learner-facing CBT claim, assessment answer, explanation, and semantic visual relation must be
  traceable to a KU/source decision. Keep the per-week coverage audit and implementation in sync.

## Approved Deep Learning workflow

Use `COGNITIVE_LEARNING_ROLLOUT_PLAN_v1.md` section 19 and `06_QA_STRATEGY.md` DoD v3. In short:

1. Full source read/extraction -> KU inventory -> Coverage Matrix -> Terminology Map.
2. Analyze representation fit and retrieval coverage; design only the hierarchy, relationships,
   process, comparison, decision logic, case work, and interaction justified by the source.
3. Prepare the source-grounded blueprint and stop for owner blueprint review.
4. Implement only the approved scope, reusing the established architecture.
5. Run technical, source, accessibility, responsive, and browser visual QA.
6. Stop for owner visual/learning review. Only after approval may the work be marked `COMPLETE` and
   confirmed metadata be integrated into the global Knowledge Map.

The deprecated “source note -> short lesson -> quiz -> complete” workflow is not acceptable.

## Locked cognitive and visual architecture

- `COGNITIVE_LEARNING_ARCHITECTURE_v1.md` v1.1 is the governing architecture. Reuse its semantic
  models, adapters, `ConceptGraph`, and `MindMapBranch`; keep rendering data-driven, not
  component-per-page bespoke — this must stay compatible with the standalone WASM app (there is no
  server to render on).
- The owner-approved Week 6 implementation is the locked project-wide Mind Map reference. The locked
  standard includes desktop spatial parent-to-child hierarchy, mobile expandable hierarchy, compact
  memory nodes, clear primary/secondary hierarchy, branch disclosure, secondary navigation, and an
  accessible fallback generated from the same data.
- The owner-approved `/kurs/karta` Course Map is locked; its CBT Knowledge Map is the reference for
  spatial network-style Concept Maps with visible, source-grounded relations.
- Treat every item recorded as `OWNER APPROVED` or `LOCKED` in current status/session history as a
  regression target. Do not polish, restructure, or change its shared engine without an explicit owner
  request. A page-local defect does not authorize a new renderer, architecture, graph library, or
  project-wide redesign.
- Week 6 is a reference, not a mechanical template. Apply Representation Fit; do not add decorative
  diagrams or force every week into the same quantity or layout of visuals.

## Mind Map is not Concept Map

- A **Mind Map** is a strict single-parent hierarchy used for orientation and memory structure. Weekly
  Preview and Review must render the same semantic `MindMapModel`; state/disclosure may differ, but
  nodes, parentage, and knowledge structure may not drift.
- A **Concept Map** is a relationship network. It may be multi-parent and uses explicit, visible,
  source-grounded relation labels. It is not a tree or a card grid.
- A **Case Conceptualization Map** is a separate domain model populated only with established case
  observations. Do not collapse these three representations into one generic content model.

## Owner-review workflow

- `READY`, `IMPLEMENTED`, and passing tests are not `OWNER APPROVED`. Only an explicit owner sign-off
  moves a result to `OWNER APPROVED`, `LOCKED`, or `COMPLETE` — never infer approval from a clean build.
- Automated and structural QA do not replace browser review. For visual work, inspect the rendered
  page at relevant breakpoints and interaction states and return fresh, clearly named screenshots.
- If browser QA is unavailable, say `Structural QA only`; never imply pixel approval. Keep the result
  `AWAITING OWNER VISUAL/LEARNING REVIEW` until the owner explicitly approves it.
- When the owner asks to stop for review, make no follow-on content, architecture, staging, commit, or
  next-phase changes. Apply review feedback narrowly and preserve approved content/relations.

## Definition of done

For code, content, test, or UI changes, unless the task explicitly narrows validation:

- Run the relevant targeted tests and the full solution test suite.
- Build `CbtLearningPlatform/CbtLearningPlatform.sln` in Debug and Release with zero errors; resolve
  new warnings rather than suppressing them. Match the Release build/test contract in CI.
- Run `git diff --check`; smoke-test affected routes and interactions; perform accessibility,
  responsive, regression, source, and visual checks proportional to the change.
- Report the commands and actual current results—never copy an old test count as a baseline.
- Update Project OS documents only when the authorized task changes architecture, decisions, status,
  roadmap, coverage, or session history. Documentation-only routing changes do not require an app build.

## Deployment

- The app is a standalone Blazor WebAssembly site — converted from a hosted Blazor Web App so it can
  deploy to Netlify, which only serves static output. There is no server project to run or deploy.
- Deployment happens exclusively through `.github/workflows/ci.yml`'s `deploy` job: a push to `main`
  makes CI build, test, publish the WASM output, and push it to Netlify (`nwtgck/actions-netlify`,
  using the `NETLIFY_SITE_ID`/`NETLIFY_AUTH_TOKEN` repo secrets).
- Application deployment must always go through GitHub Actions — never push the app manually (no local
  `netlify deploy`, no bypassing `ci.yml`). If the deploy mechanism itself needs to change, change
  `ci.yml` and let the normal push -> CI -> Netlify path run it.
- Netlify/GitHub account or site configuration (secrets, site settings, DNS, replacing the site) is
  separate from application deployment — only change it when the owner explicitly asks for that
  specific change; never as a side effect of an unrelated task.

## Git isolation and protected working state

- Existing staged, unstaged, and untracked changes belong to the owner or another task. Never reset,
  restore, stash, delete, reformat, stage, unstage, amend, or absorb them without explicit permission.
- Avoid `git add .` and broad commits. Stage only task-owned paths; verify both working-tree and cached
  diffs (`git status --short`, `git diff --stat`, `git diff --cached --stat`) before every commit.
  Commit only when asked, using explicit path isolation when unrelated staged/unstaged work exists.
- This file never records which commits or protected batches are pending — that state changes too
  often to keep here. Re-check live git state on every task instead; if something is actually pending,
  say so in your own report, not as a persistent edit to this file.

## Authoritative documents

- Current position and lock roster: `00_PROJECT_OS/02_CURRENT_STATUS.md`.
- Durable decisions: `00_PROJECT_OS/03_DECISION_LOG.md`.
- QA and Deep Learning DoD v3: `00_PROJECT_OS/06_QA_STRATEGY.md`.
- Content sourcing and safety: `00_PROJECT_OS/07_CONTENT_GOVERNANCE.md`,
  `00_PROJECT_OS/11_SOURCE_REGISTER.md`, and `00_PROJECT_OS/23_CLINICAL_SAFETY_BOUNDARIES.md`.
- Project-level source inventory: `00_PROJECT_OS/12_SOURCE_COVERAGE_MATRIX.md`; authoritative
  week-level KU accounting lives in the matching `_blueprints/WEEK_*_SOURCE_COVERAGE_AUDIT*.md`.
- Cognitive semantics and visual standards:
  `00_PROJECT_OS/_blueprints/COGNITIVE_LEARNING_ARCHITECTURE_v1.md`.
- Approved rollout and future-week workflow:
  `00_PROJECT_OS/_blueprints/COGNITIVE_LEARNING_ROLLOUT_PLAN_v1.md`.
- Sequencing and evidence of owner approvals: `00_PROJECT_OS/24_IMPLEMENTATION_ROADMAP.md` and
  `00_PROJECT_OS/10_SESSION_LOG.md`.
- Product scope: `00_PROJECT_OS/00_PROJECT_CHARTER.md` and
  `00_PROJECT_OS/17_PRODUCT_REQUIREMENTS_DOCUMENT.md`.
