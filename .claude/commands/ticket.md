---
description: Azure DevOps ticket → branch → implement + tests → commit → GitHub PR with reviewer
argument-hint: <ticket-number> [reviewer-github-username] [base-branch]
---

# Azure DevOps Ticket → GitHub PR

Arguments: `$ARGUMENTS`
- 1st: Azure DevOps ticket (work item) number — required
- 2nd: reviewer GitHub username — optional
- 3rd: base branch — optional (default: repo default branch, `gh repo view --json defaultBranchRef -q .defaultBranchRef.name`)

Goal: read the ticket, implement it in THIS repo, and stop once the PR is open with a reviewer assigned. Do NOT address review comments, merge, or close the ticket.

Reply in the user's language (Hinglish is fine). Use a todo list to track phases.

## Conventions (edit these for your team)

- Branch: `feature/<ticket>-<short-kebab-description>` (use `bugfix/` for Bug work items)
- Commit / PR title: `AB#<ticket>: <short imperative summary>`
- Default reviewer: Shambhu2612
- Azure DevOps: org `letsgotocloud`, project `AIPoweredEcomm` (Basic process: To Do / Doing / Done)
- Ticket states: In Progress → `Doing`; In Review → keep `Doing` and add a PR-link comment (never set `Done` — that happens after merge)
- Stack: .NET (ASP.NET Core). Build: `dotnet build`, tests: `dotnet test`, format: `dotnet format --verify-no-changes`
- Empty repo: if the base branch has no commits, ask before creating an initial commit on it (.gitignore + README only), push it, then branch from it.

## 0. Inputs

If the ticket number is missing, ask for it. If no reviewer is given and no default is set above, ask for it. Ask for everything missing in ONE message.

## Accessing Azure DevOps

1. Prefer the `azure-devops` MCP server tools (configured in `.mcp.json`) to read/update work items and add comments.
2. If MCP is unavailable, fall back to Azure CLI: `az boards work-item show --id <ticket> -o json` / `az boards work-item update --id <ticket> --state <state>` (needs `az extension add --name azure-devops` and `az login`).
3. If neither works, ask the user to paste the ticket details. Do not guess requirements.

## Phase 1 — Understand

1. Read the work item: title, type, description, acceptance criteria, comments, attachments, linked/parent items.
2. Check for an existing branch/PR: `git branch -a | grep -i <ticket>`, `gh pr list --state all --search "<ticket>"`.
3. Explore the codebase: affected modules, existing patterns, test framework, lint/format/type-check/build commands (package.json, Makefile, pyproject, CI workflows, CONTRIBUTING, CLAUDE.md).
4. Write the analysis:
   - What changes and why
   - Files/modules to touch
   - Edge cases (empty/null, boundaries, invalid formats, duplicates, permissions, concurrency, errors/timeouts, backward compatibility, large data)
   - Test plan
   - Migration / config / env changes
   - Open questions
5. **CHECKPOINT 1 — STOP.** Show analysis + plan and wait for the user's approval. Resolve open questions here.
6. Set the ticket to the In Progress state.

## Phase 2 — Setup

7. Ensure a clean working tree (`git status --porcelain`); if dirty, stop and ask.
8. `git fetch origin && git checkout <base> && git pull --ff-only origin <base>`
9. `git checkout -b <branch-name>`

## Phase 3 — Implement

10. Implement following existing style and patterns; handle every edge case from the analysis.
11. Note any DB migration / config / env variable changes.
12. Update README / API docs / CHANGELOG if behavior or interfaces change.

## Phase 4 — Test

13. Write tests for the main flow and each edge case, in the project's existing framework and layout.
14. Run the FULL test suite. Fix failures; never delete or skip existing tests to make them pass.
15. Check coverage for changed code if the project supports it.

## Phase 5 — Cleanup & quality

16. Remove unused code, debug logs, commented-out code, stray TODOs you added.
17. Run linter, formatter, type checker, build — all must pass.
18. Scan the diff for secrets (keys, tokens, passwords, connection strings). Never commit `.env` or credential files.
19. Self-review: `git diff <base>...HEAD` and `git status`.

## Phase 6 — Commit

20. Stage only relevant files (`git add <paths>`, not blind `git add .`).
21. `git commit -m "AB#<ticket>: <summary>"` (add a body for non-trivial changes).
22. **CHECKPOINT 2 — STOP.** Show diff summary, test/lint/build results and the commit. Wait for approval before pushing.

## Phase 7 — Push & PR

23. `git fetch origin && git rebase origin/<base>`; resolve conflicts and re-run tests if anything changed.
24. `git push -u origin <branch-name>`
25. Write the PR body to a temp file with sections:
    - **Ticket:** `AB#<ticket>` + work item link
    - **What changed and why**
    - **Edge cases covered**
    - **How it was tested** (commands + results)
    - **Migration / config / env changes** (or "None")
26. `gh pr create --base <base> --head <branch-name> --title "AB#<ticket>: <summary>" --body-file <file> --reviewer <reviewer>`
    Use `--draft` only if the user says the work is incomplete.
27. `gh pr checks <pr-number>` — report CI status; fix only if clearly caused by this change and the user agrees.
28. Azure DevOps: set the ticket to the In Review state and add a comment with the PR link.

## Finish

Report in a few lines: PR link, branch, reviewer, CI status, anything needing attention. Stop.

## Rules

- Never push or open a PR without Checkpoint 2 approval.
- Never commit to or force-push the base branch.
- If a step fails twice (auth, CLI, CI tooling), stop and tell the user what is blocked.
