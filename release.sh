rm -dfr release

targetDir=release/moff

mkdir -p $targetDir
rm -dfr moff/build/tmp
cp -Rpv moff/build/* $targetDir

rm $targetDir/*.xml
rm $targetDir/*.pdb
rm $targetDir/System.ValueTuple.*

cp -pv readme.md $targetDir
