import graph_gen as gen
import plant as pl
import image_transfer as img_t
import sys
import fnmatch
import os





arguments = sys.argv[1:]

if (len(arguments)!=1):
    print("Arguments incorrect.")
    
else:

    if(arguments[0]=="all"):
        print("Transferring all")
        img_t.transfer_all()

    elif(arguments[0]=="down"):
        img_t.download_all()
    
    elif(fnmatch.fnmatch(arguments[0], '*.png')):

        graph_dir_path = 'PyScripts\\Graphs'
        graph_list = []

        for file_path in os.listdir(graph_dir_path):
            # check if current file_path is a file
            if os.path.isfile(os.path.join(graph_dir_path, file_path)):
                # add filename to list
                graph_list.append(file_path)
        if(arguments[0] not in graph_list):
            print("File is not in folder.")
        else:
            print("Transferring "+arguments[0])
            img_t.transfer_one(arguments[0])

    elif(arguments[0]=='overview'):
        print("Generating overview graphs.")
        gen.overview_data()
    elif(arguments[0]=='specific'):
        print("Generating specific graphs.")
        all_data = gen.concat_all_data()
        gen.specific_data(all_data)
    else:
        print("Arguments incorrect.")

