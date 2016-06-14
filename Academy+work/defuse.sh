touch -A -000001 bomb.txt
date -r bomb.txt | grep -o -E "..:..:.."
