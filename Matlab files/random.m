
% Output datas
output_resized= '/Users/pietrosandra/matlab/classification/bank_one_hand/right/'; % #TO MODIFY

% Create an image datastore with the input image
imds = imageDatastore('/Users/pietrosandra/matlab/classification/one_hand/right/'); % #TO MODIFY

% Conversion de las imagenes
num_train_files = 2397; %nb of pictures in the input folder
L = randperm(num_train_files); %allow to name files randomly and to mix the frames
j=0;
while hasdata(imds)
    j=j+1;
    im = read(imds); 
    output_filename = sprintf('right_%04d.jpg',L(1,j)); %specify the output filename #TO MODIFY
    output_path = fullfile(output_resized,output_filename); %construct the full output path
    imwrite(im,output_path); %save as an image file
end