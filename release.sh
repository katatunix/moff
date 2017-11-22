rm -dfr moff/build/tmp
cp -Rpv moff/build .
rm build/*.xml
rm build/*.pdb
rm build/System.ValueTuple.*
cp -pv readme.md build
