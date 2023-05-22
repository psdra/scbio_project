
%% Code for training:

%init 
clc
clear all
close all
warning off

%call of the neuronal network alexnet
g=alexnet;
layers=g.Layers;
numClasses=4;
layers(23)=fullyConnectedLayer(numClasses);
layers(25)=classificationLayer;

%load data
digitDatasetPath_training = fullfile('/Users/pietrosandra/matlab/classification/bank_one_hand/');
imdsTrain = imageDatastore(digitDatasetPath_training, ...
    'IncludeSubfolders',true, ...
    'LabelSource','foldernames');

data augmentation step 
augmenter = imageDataAugmenter(...
    'RandRotation', [-10 10],...
    'RandXTranslation', [-10 10],...
    'RandYTranslation', [-10 10],...
    'RandXShear', [-5 5],...
    'RandYShear', [-5 5],...
    'RandScale', [0.9 1.1]);
imds_Train = augmentedImageDatastore([227 227 3], imdsTrain, 'DataAugmentation', augmenter);

%training
opts=trainingOptions('sgdm','InitialLearnRate',0.001,'MaxEpochs',4,'MiniBatchSize',64);
clasificador2=trainNetwork(imdsTrain,layers,opts);

%saving
save clasificador2;