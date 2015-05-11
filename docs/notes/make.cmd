@echo off

:: -f markdown_github-hard_line_breaks+ignore_line_breaks+yaml_metadata_block
:: --base-header-level=1
:: --number-sections
:: --number-offset=1

pandoc ^
  --standalone ^
  --smart ^
  --toc ^
  --toc-depth=3 ^
  --no-highlight ^
  -f markdown+yaml_metadata_block ^
  -t html5 ^
  -c assets/main.css ^
  -H assets/header.html ^
  -A assets/footer.html ^
  monads.md ^
  monads.yaml ^
  -o monads.html

@exit /B %ERRORLEVEL%
