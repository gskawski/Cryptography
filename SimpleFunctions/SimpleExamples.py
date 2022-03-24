# affine cipher
import string



def affine_cipher(): 
    affine_str = 'UqpLdsGpmzaxbabclByjfcbbsfsghbagabbrhcpfu'.lower()
    a = 9
    b = 5
    keys = list(map(lambda plain: (plain, chr(((((ord(plain)-97)*a)+b)%26)+97)), string.ascii_lowercase))
    print(''.join(list(map(lambda cipher: keys[[y[1] for y in keys].index(cipher)][0], affine_str))))


def caesar_cipher(encrypt_str):
    for shift in range(27):
        print("{}: ".format(shift) + ''.join(list(map(lambda cipher_char: chr(97+((26+(ord(cipher_char)-97)-shift)%26)), encrypt_str.lower()))))


affine_cipher()

q4str = "jxuauovehrhuqaydwoekhdunjsyfxuhyihywxjkdtuhoekhdeiu"


caesar_cipher(q4str)

