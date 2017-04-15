@echo off

pandoc ^
  --template=assets/layout.html5 ^
  --standalone ^
  --smart ^
  --toc ^
  --toc-depth=2 ^
  --no-highlight ^
  -f markdown_github-hard_line_breaks+emoji ^
  -t html5 ^
  ../../src/Narvalo.Fx/README.md ^
  -o README.html

@exit /B %ERRORLEVEL%
