 %% Code for testing:

clc;
close all;
clear all
warning off
c=webcam;
load clasificador2;
x=0;
y=0;
height=700;
width=700;
bboxes=[x y height width];
h = figure;
while ishandle(h)
    pause(1)
    e=c.snapshot;
    IFaces = insertObjectAnnotation(e,'rectangle',bboxes,'Processing Area');   
    es=imcrop(e,bboxes);
    es=imresize(es,[227 227]);
    label=classify(clasificador2,es);
    imshow(IFaces);
    title(char(label));
    drawnow;
    SendData(string(label));
end
clear c