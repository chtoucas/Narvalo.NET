@echo off

pandoc ^
  --template=assets/layout.html5 ^
  --standalone ^
  --smart ^
  --toc ^
  --toc-depth=3 ^
  --no-highlight ^
  -f markdown_github-hard_line_breaks+emoji ^
  -t html5 ^
  content/narvalo-fx.md ^
  -o narvalo-fx.html

@exit /B %ERRORLEVEL%
