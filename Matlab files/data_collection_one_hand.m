%% Code for Data Collection:

clc
clear all
close all
warning off
c=webcam;
pause(1);
x=0;
y=0;
height=700;
width=700;
bboxes=[x y height width];
temp=0;
output_folder= '/Users/pietrosandra/matlab/classification/one_hand/drop/';
num_train_files=300;
L = randperm(num_train_files); %allow to name files randomly and to mix the frames
while temp<num_train_files
    temp=temp+1;
    e=c.snapshot;
    IFaces = insertObjectAnnotation(e,'rectangle',bboxes,'Processing Area');   
    imshow(IFaces);
    es=imcrop(e,bboxes);
    es=imresize(es,[227 227]);
    output_filename = sprintf('drop%04d.jpg',2100+L(1,temp)); %specify the output filename
    output_path = fullfile(output_folder,output_filename); %construct the full output path
    imwrite(es,output_path);
    drawnow;
end
clear c;