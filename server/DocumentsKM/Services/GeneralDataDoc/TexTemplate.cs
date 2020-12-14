namespace DocumentsKM.Services
{
    public static class TexTemplate
    {
        public static readonly string docTop = @"\documentclass[12pt, a4paper]{article}
\usepackage[utf8]{inputenc}
\usepackage{hyperref}
\usepackage{array}
\usepackage{lastpage}
\usepackage{placeins}
\usepackage[T2A]{fontenc}
\usepackage[english, russian]{babel}
\usepackage{multirow}
\usepackage{dblfloatfix}
\usepackage{booktabs}
\usepackage{changepage}
\usepackage{enumitem}
\usepackage{gensymb}
\usepackage{eso-pic}
\usepackage{fancyhdr}
\usepackage{ifthen}
\usepackage{tikz}
\usepackage{afterpage}
\usepackage[top=15mm,bottom=75.2mm,left=28mm,right=12mm,footskip=46.6mm]{geometry}

\usetikzlibrary{calc}

% Removes header
\renewcommand{\headrulewidth}{0pt}
\renewcommand{\footrulewidth}{0pt}

\setlength{\parindent}{0.95cm}

% Footer
\pagestyle{fancy}
\renewcommand{\headrulewidth}{0pt}
\fancyhead[C,CE,CO,LE,LO,RE,RO]{} %% clear out all headers

\fancyfoot[CE,CO,LE,LO,RE,RO]{} %% clear out all footers
\fancyfoot[C]{\ifthenelse{\value{page}=1}{{\itshape
\vspace{-73.7mm}
        \hspace*{-6mm}\begin{tabular}{m{5.15mm}!{\vrule width 0.5mm}m{5.15mm}!{\vrule width 0.5mm}m{5.15mm}!{\vrule width 0.5mm}m{5.15mm}!{\vrule width 0.5mm}m{10.15mm}!{\vrule width 0.5mm}m{5.15mm}!{\vrule width 0.5mm}P{63.15mm}!{\vrule width 0.5mm}P{10.15mm}!{\vrule width 0.5mm}P{10.15mm}!{\vrule width 0.5mm}P{15.15mm}}
\specialrule{0.5mm}{0pt}{0pt}
& & & & & & \multicolumn{4}{c}{\multirow{2}{*}{М28541.238К-КМ2}}\\\cline{1-6}
& & & & & & \multicolumn{4}{c}{}\\\cline{1-6}\Cline{7-10}
& & & & & & \multicolumn{4}{c}{\multirow{3}{*}{ПАО \textquotedbl{}ММК\textquotedbl{}. ГОП. Аглоцех. Капремонт}}\\\cline{1-6}
& & & & & & \multicolumn{4}{c}{}\\\Cline{1-6}
\footnotesize{Изм.} & \footnotesize{Кол. уч.} & \footnotesize{Лист} & \footnotesize{№ док} & \footnotesize{Подп.} & \footnotesize{Дата} & \multicolumn{4}{c}{}\\\specialrule{0.5mm}{0pt}{0pt}
\multicolumn{2}{c!{\vrule width 0.5mm}}{Разраб.} & \multicolumn{2}{c!{\vrule width 0.5mm}}{Влад} & & & \multirow{3}{*}{УУк и КДИ.} & Стадия & Лист & Листов\\\cline{1-6}\Cline{8-10}
\multicolumn{2}{c!{\vrule width 0.5mm}}{} & \multicolumn{2}{c!{\vrule width 0.5mm}}{} & & & & \multirow{2}{*}{P} & \multirow{2}{*}{1.1} & \multirow{2}{*}{6}\\\cline{1-6}
\multicolumn{2}{c!{\vrule width 0.5mm}}{} & \multicolumn{2}{c!{\vrule width 0.5mm}}{} & & & & & & \\\cline{1-6}\Cline{7-10}
\multicolumn{2}{c!{\vrule width 0.5mm}}{} & \multicolumn{2}{c!{\vrule width 0.5mm}}{} & & & \multirow{3}{*}{Общие данные (на 7 листах)} & \multicolumn{3}{c}{\multirow{3}{*}{АО \textquotedbl{}МГ\textquotedbl{}}}\\\cline{1-6}
\multicolumn{2}{c!{\vrule width 0.5mm}}{} & \multicolumn{2}{c!{\vrule width 0.5mm}}{} & & & & \multicolumn{3}{c}{}\\\cline{1-6}
\multicolumn{2}{c!{\vrule width 0.5mm}}{} & \multicolumn{2}{c!{\vrule width 0.5mm}}{} & & & & \multicolumn{3}{c}{}\\\specialrule{0.5mm}{0pt}{0pt}
\end{tabular}
    \FloatBarrier
    \vspace{-0.88mm}
    \hspace*{-6mm}\begin{tabular}{m{18.15mm}|m{18.15mm}|m{18.15mm}|m{18.15mm}!{\vrule width 0.5mm}m{18.15mm}|m{18.15mm}|m{18.15mm}|m{18.15mm}}
\specialrule{0.5mm}{0pt}{0pt}
\multicolumn{4}{l!{\vrule width 0.5mm}}{Согласовано:} & & & &\\\Cline{1-4}\cline{5-8}
Зав. гр. & Осинцев & & & & & & \\\hline
& & & & & & & \\\hline
& & & & & & & \\\specialrule{0.5mm}{0pt}{0pt}
\end{tabular}
    \FloatBarrier
    \vspace{-0.88mm}
    \hspace*{-6mm}\begin{tabular}{m{60.2mm}!{\vrule width 0.5mm}m{63.2mm}!{\vrule width 0.5mm}m{44.2mm}}
    \specialrule{0.5mm}{0pt}{0pt}
    Инв. № подл. & Подп. и дата & Взам. инв. №\\
    & & \\\specialrule{0.5mm}{0pt}{0pt}
    \end{tabular}
}}{{\itshape
        \hspace*{-6mm}\begin{tabular}{m{5.4mm}!{\vrule width 0.5mm}m{5.4mm}!{\vrule width 0.5mm}m{5.4mm}!{\vrule width 0.5mm}m{5.4mm}!{\vrule width 0.5mm}m{10.4mm}!{\vrule width 0.5mm}m{5.4mm}!{\vrule width 0.5mm}P{102.1mm}!{\vrule width 0.5mm}m{4.1mm}}
    \specialrule{0.5mm}{0pt}{0pt}
    & & & & & &\multirow{3}{*}{М28541.238К-КМ2} & \footnotesize{Лист}\tabularnewline\cline{1-6}\Cline{8-8}
    & & & & & & & 1.\thepage \tabularnewline\Cline{1-6}
    \footnotesize{Изм.} & \footnotesize{Кол. уч.} & \footnotesize{Лист} & \footnotesize{№ док} & \footnotesize{Подп.} & \footnotesize{Дата} & & \tabularnewline\specialrule{0.5mm}{0pt}{0pt}
    \end{tabular}
    \FloatBarrier
    \vspace{-0.88mm}
    \hspace*{-6mm}\begin{tabular}{m{56.15mm}!{\vrule width 0.5mm}m{56.15mm}!{\vrule width 0.5mm}m{55.15mm}}
    \specialrule{0.5mm}{0pt}{0pt}
    Инв. № подл. & Подп. и дата & Взам. инв. №\tabularnewline
    & & \tabularnewline\specialrule{0.5mm}{0pt}{0pt}
    \end{tabular}
}}}

% Thick hline
\newlength\Origarrayrulewidth
\newcommand{\Cline}[1]{%
\noalign{\global\setlength\Origarrayrulewidth{\arrayrulewidth}}%
\noalign{\global\setlength\arrayrulewidth{0.5mm}}\cline{#1}%
\noalign{\global\setlength\arrayrulewidth{\Origarrayrulewidth}}%
}

% Centered table cell type
\newcolumntype{P}[1]{>{\centering\arraybackslash}p{#1}}

% List identation
\setlist{itemindent=\dimexpr\labelwidth+\labelsep\relax,leftmargin=0pt, wide=20pt}
%\setlist[enumerate]{label=\theenumi}

% Disables word breaks
\tolerance=1
\emergencystretch=\maxdimen
\hyphenpenalty=10000
\hbadness=10000

\renewcommand\labelitemi{--}
\renewcommand{\labelenumii}{\theenumii}
\renewcommand{\theenumii}{\theenumi.\arabic{enumii}}

\begin{document}
\enlargethispage{-60mm}

\AddToShipoutPicture{%
\begin{tikzpicture}
[remember picture, overlay] \draw[line width=0.5mm] ($(current page.north west) + (22mm,-6mm)$) rectangle ($(current page.south east) + (-7mm,9mm)$);
\end{tikzpicture}
}

{\itshape
\begin{center}
\large{\textbf{Общие указания}}
\end{center}

\begin{flushleft}
\begin{enumerate}[label*=\arabic*]";

        // ******************************************************************************

        public static readonly string docBottom = @"\end{enumerate}
\end{flushleft}

\afterpage{%
\newgeometry{left=28mm,right=12mm,top=20mm,bottom=75.2mm}

{{\itshape
\begin{center}
\large{\textbf{Ведомость рабочих чертежей основного комплекта}}
\end{center}
\setlength\arrayrulewidth{0.25mm}
\begin{adjustwidth}{-6mm}{0mm}
\begin{tabular}{P{10.15mm}!{\vrule width 0.5mm}m{138.15mm}!{\vrule width 0.5mm}m{19.15mm}}
\specialrule{0.5mm}{0pt}{0pt}
\multirow{3}{*}{Лист} & \multirow{3}{*}{Наименование} & \multirow{3}{*}{Примечание}\\
& & \\
& & \\\specialrule{0.5mm}{0pt}{0pt}
\multirow{2}{*}{1} & \multirow{2}{*}{1} & \multirow{2}{*}{1} \\
& & \\\hline
\multirow{2}{*}{1} & \multirow{2}{*}{1} & \multirow{2}{*}{1} \\
& & \\\specialrule{0.5mm}{0pt}{0pt}
\end{tabular}
\end{adjustwidth}
}}

\clearpage

{{\itshape
\begin{center}
\large{\textbf{Ведомость ссылочных и прилагаемых документов}}
\end{center}
\setlength\arrayrulewidth{0.25mm}
\begin{adjustwidth}{-6mm}{0mm}
\begin{tabular}{m{56.5mm}!{\vrule width 0.5mm}m{90.5mm}!{\vrule width 0.5mm}m{20.5mm}}
\specialrule{0.5mm}{0pt}{0pt}
\multirow{3}{*}{Обозначение} & \multirow{3}{*}{Наименование} & \multirow{3}{*}{Примечание}\\
& & \\
& & \\\specialrule{0.5mm}{0pt}{0pt}
\multirow{2}{*}{1} & \multirow{2}{*}{1} & \multirow{2}{*}{1} \\
& & \\\hline
\multirow{2}{*}{1} & \multirow{2}{*}{1} & \multirow{2}{*}{1} \\
& & \\\specialrule{0.5mm}{0pt}{0pt}
\end{tabular}
\end{adjustwidth}
}}
\restoregeometry
}


\end{document}";
    }
}
