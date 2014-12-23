
.PHONY: all pdf dvi ps clean distclean

all: pdf

pdf: Guidelines.tex
	pdflatex $?

clean:
	rm -f *.aux *.idx *.log *.lot *.out *.toc

distclean: clean
	rm -f *.pdf
