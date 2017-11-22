rm -dfr moff/build/tmp
cp -R moff/build .
rm build/*.xml
rm build/*.pdb
rm build/System.ValueTuple.*
cp readme.md build
