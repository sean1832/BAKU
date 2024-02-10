import os
import zipfile
from xml.etree import ElementTree as ET
import fnmatch
import argparse

def get_project_info(project_file_path):
    """
    Extracts project name and version from the .csproj file.
    """
    tree = ET.parse(project_file_path)
    root = tree.getroot()
    project_name = root.find('.//Title')
    version = root.find('.//Version')
    return project_name.text if project_name is not None else "Project", version.text if version is not None else "1.0.0"

def zip_files(project_dir, project_file, file_patterns=['*.gha', '*.dll']):
    """
    Zips files matching given patterns in the project directory's release/bin folder,
    names the zip file based on project name and version, and places them in a directory
    within the zip file named after the project.
    """
    project_name, version = get_project_info(project_file)
    zip_file_name = f"{project_name}_v{version}.zip"
    zip_path = os.path.join(project_dir, 'bin', 'release', zip_file_name)
    project_folder_in_zip = f"{project_name}/"  # Folder name inside the zip

    with zipfile.ZipFile(zip_path, 'w', zipfile.ZIP_DEFLATED) as zipf:
        for folder_name, _, filenames in os.walk(os.path.join(project_dir, 'bin', 'release', 'net48')):
            for filename in filenames:
                if any(fnmatch.fnmatch(filename, pattern) for pattern in file_patterns):
                    file_path = os.path.join(folder_name, filename)
                    zipf.write(file_path, project_folder_in_zip + os.path.basename(file_path))  # Prepend the project folder name

    return zip_path

if __name__ == "__main__":
    parser = argparse.ArgumentParser(description='Builds a Grasshopper plugin.')
    parser.add_argument('project_dir', type=str, help='The project directory.')
    parser.add_argument('project_file', type=str, help='The project file.')

    args = parser.parse_args()
    project_dir = args.project_dir
    project_file = args.project_file
    if not os.path.exists(project_dir):
        raise FileNotFoundError(f"Project directory not found: {project_dir}")
    if not os.path.exists(project_file):
        raise FileNotFoundError(f"Project file not found: {project_file}")
    if not project_file.endswith('.csproj'):
        raise ValueError(f"Invalid project file: {project_file}")
    if not os.path.exists(os.path.join(project_dir, 'bin', 'release', 'net48')):
        raise FileNotFoundError(f"Release/bin/net48 folder not found in project directory: {project_dir}")

    zip_path = zip_files(project_dir, project_file)
    print(f"Files zipped successfully: {zip_path}")