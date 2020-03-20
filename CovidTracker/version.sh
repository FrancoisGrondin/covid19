#!/bin/bash

app="CovidTracker"

if [ $# -ne 1 ]; then
    echo "Current version: `grep VERSION_NAME ${app}/AppConfiguration.cs | cut -d\\" -f2`"
    echo "Usage: `basename $0` [new version]"
    exit 1
fi

lib=`echo ${app} | awk '{print toupper(substr($0,0,1))tolower(substr($0,2))}'`

version=${1}
codeline=`echo ${version} | sed -e 's/\./ /g'`
code=`for d in ${codeline}; do echo -n $(printf "%02d" ${d}); done`

echo "Version: ${version}"
echo "Code   : ${code}" 

sed -i "" 's/version = .*/version = '${version}'/g' ${app}.sln

sed -i "" 's/<ReleaseVersion>.*<\/ReleaseVersion>/<ReleaseVersion>'${version}'<\/ReleaseVersion>/g' ${app}/${app}.csproj
sed -i "" 's/<ReleaseVersion>.*<\/ReleaseVersion>/<ReleaseVersion>'${version}'<\/ReleaseVersion>/g' ${app}.iOS/${app}.iOS.csproj
sed -i "" 's/<ReleaseVersion>.*<\/ReleaseVersion>/<ReleaseVersion>'${version}'<\/ReleaseVersion>/g' ${app}.Android/${app}.Android.csproj

sed -i "" 's/android:versionName="[0-9][0-9]*.[0-9][0-9]*.[0-9][0-9]*"/android:versionName="'${version}'"/g' ${app}.Android/Properties/AndroidManifest.xml
sed -i "" 's/android:versionCode="[0-9][0-9]*"/android:versionCode="'${code}'"/g' ${app}.Android/Properties/AndroidManifest.xml

echo -e "<key>CFBundleVersion</key>\n\t<string>${code}</string>" > /var/tmp/tmpstring
perl -i -p0e 's/<key>CFBundleVersion<\/key>\n\t<string>[0-9][0-9]*<\/string>/`cat \/var\/tmp\/tmpstring`/se' ${app}.iOS/Info.plist 
echo -e "<key>CFBundleShortVersionString</key>\n\t<string>${version}</string>" > /var/tmp/tmpstring
perl -i -p0e 's/<key>CFBundleShortVersionString<\/key>\n\t<string>[0-9][0-9]*.[0-9][0-9]*.[0-9][0-9]*<\/string>/`cat \/var\/tmp\/tmpstring`/se' ${app}.iOS/Info.plist
cat ${app}.iOS/Info.plist | grep -v ^$ > /var/tmp/Info.plist
\mv /var/tmp/Info.plist ${app}.iOS/Info.plist
\rm /var/tmp/tmpstring

sed -i "" 's/VERSION_NAME = .*/VERSION_NAME = "'${version}'";/g' ${app}/AppConfiguration.cs
sed -i "" 's/VERSION_CODE = .*/VERSION_CODE = '${code}';/g' ${app}/AppConfiguration.cs

