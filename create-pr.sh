#!/bin/bash

set -e

git checkout main
git pull
git checkout -b refactor/setup-improvements
git add .
git commit -m "Refactor startup, fix Nutrition fallback, add Docker support, improve config"
git push -u origin refactor/setup-improvements

gh pr create \
  --title "Refactor startup and deployment setup" \
  --body "This PR refactors the startup configuration and adds:\n\n- Conditional Cosmos DB registration\n- Auto EF migrations in dev\n- Nutrition fallback fix\n- ClaimsPrincipal extension\n- Docker support\n- Config placeholders updated\n\n✅ Backward compatible. Please review and merge when ready." \
  --base main \
  --head refactor/setup-improvements
