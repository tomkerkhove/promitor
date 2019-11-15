change: init_githooks
	cd site/changelog && hugo new experimental/`shuf -i 10000-60000 -n 1`.md

deprecation: init_githooks
	cd site/changelog && hugo new deprecated/`shuf -i 10000-60000 -n 1`.md

release_create:
	python release.py

release: release_create
	git push
	git push --tags

generate_changelog:
	cd site/changelog && hugo

serve_changelog:
	cd site/changelog && hugo server --bind 0.0.0.0 --baseURL=http://localhost/changelog/

init_githooks:
	@git config core.hooksPath .githooks
