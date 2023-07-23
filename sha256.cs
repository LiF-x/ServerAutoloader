
if (!isObject(LiFxSHA256))
{
    new ScriptObject(LiFxSHA256)
    {
    };
}
// implemented using pseudocode from https://en.wikipedia.org/wiki/SHA-2#Pseudocode
// inline variant of Port's add function from https://forum.blockland.us/index.php?topic=248922.0
// tested against SHA256 test vectors from http://www.di-mgt.com.au/sha_testvectors.html
$ASCIITable="\x01\x02\x03\x04\x05\x06\x07\x08\x09\x0A\x0B\x0C\x0D\x0E\x0F"@
	"\x10\x11\x12\x13\x14\x15\x16\x17\x18\x19\x1A\x1B\x1C\x1D\x1E\x1F"@
	"\x20\x21\x22\x23\x24\x25\x26\x27\x28\x29\x2A\x2B\x2C\x2D\x2E\x2F"@
	"\x30\x31\x32\x33\x34\x35\x36\x37\x38\x39\x3A\x3B\x3C\x3D\x3E\x3F"@
	"\x40\x41\x42\x43\x44\x45\x46\x47\x48\x49\x4A\x4B\x4C\x4D\x4E\x4F"@
	"\x50\x51\x52\x53\x54\x55\x56\x57\x58\x59\x5A\x5B\x5C\x5D\x5E\x5F"@
	"\x60\x61\x62\x63\x64\x65\x66\x67\x68\x69\x6A\x6B\x6C\x6D\x6E\x6F"@
	"\x70\x71\x72\x73\x74\x75\x76\x77\x78\x79\x7A\x7B\x7C\x7D\x7E\x7F"@
	"\x80\x81\x82\x83\x84\x85\x86\x87\x88\x89\x8A\x8B\x8C\x8D\x8E\x8F"@
	"\x90\x91\x92\x93\x94\x95\x96\x97\x98\x99\x9A\x9B\x9C\x9D\x9E\x9F"@
	"\xA0\xA1\xA2\xA3\xA4\xA5\xA6\xA7\xA8\xA9\xAA\xAB\xAC\xAD\xAE\xAF"@
	"\xB0\xB1\xB2\xB3\xB4\xB5\xB6\xB7\xB8\xB9\xBA\xBB\xBC\xBD\xBE\xBF"@
	"\xC0\xC1\xC2\xC3\xC4\xC5\xC6\xC7\xC8\xC9\xCA\xCB\xCC\xCD\xCE\xCF"@
	"\xD0\xD1\xD2\xD3\xD4\xD5\xD6\xD7\xD8\xD9\xDA\xDB\xDC\xDD\xDE\xDF"@
	"\xE0\xE1\xE2\xE3\xE4\xE5\xE6\xE7\xE8\xE9\xEA\xEB\xEC\xED\xEE\xEF"@
	"\xF0\xF1\xF2\xF3\xF4\xF5\xF6\xF7\xF8\xF9\xFA\xFB\xFC\xFD\xFE\xFF";
package LiFxSHA256 {
  function LiFxSHA256::hashMain(%file)
  {
    %totallines = "";
    // Create a file object for reading
    %fsObject = new FileObject();
    %fsObject.OpenForRead(%file);
    // Open a text file, if it exist
    // Keep reading until we reach the end of the file
    while( !%fsObject.isEOF() )
    {
      %line = %fsObject.readLine();
      if(%totallines !$= "")
      {
        %totallines = %totallines NL %line;
      }
      else {
        %totallines = %line;
      }

    }
    %fsObject.close();
    %fsObject.delete();
    return LiFxSHA256::hash(%totallines);
        // Close the file when finished

    // Cleanup the file object
    
  }
  function LiFxSHA256::add(%a, %b) {
    return ( ( %a | 0 ) + ( %b | 0 ) ) | 0;
  }

  function LiFxSHA256::hash(%text)
  {
      
    // initialize hash values:
    // first 32 bits of the fractional parts of the square roots of the first 8 primes (2 through 19):
    %h0 = 0x6A09E667;
    %h1 = 0xBB67AE85;
    %h2 = 0x3C6EF372;
    %h3 = 0xA54FF53A;
    %h4 = 0x510E527F;
    %h5 = 0x9B05688C;
    %h6 = 0x1F83D9AB;
    %h7 = 0x5BE0CD19;
    
    // initialize array of round constants:
    // first 32 bits of the fractional parts of the cube roots of the first 64 primes (2 through 311):
    %k[0] = 0x428A2F98; %k[1] = 0x71374491; %k[2] = 0xB5C0FBCF; %k[3] = 0xE9B5DBA5;
    %k[4] = 0x3956C25B; %k[5] = 0x59F111F1; %k[6] = 0x923F82A4; %k[7] = 0xAB1C5ED5;
    %k[8] = 0xD807AA98; %k[9] = 0x12835B01; %k[10] = 0x243185BE; %k[11] = 0x550C7DC3;
    %k[12] = 0x72BE5D74; %k[13] = 0x80DEB1FE; %k[14] = 0x9BDC06A7; %k[15] = 0xC19BF174;
    %k[16] = 0xE49B69C1; %k[17] = 0xEFBE4786; %k[18] = 0x0FC19DC6; %k[19] = 0x240CA1CC;
    %k[20] = 0x2DE92C6F; %k[21] = 0x4A7484AA; %k[22] = 0x5CB0A9DC; %k[23] = 0x76F988DA;
    %k[24] = 0x983E5152; %k[25] = 0xA831C66D; %k[26] = 0xB00327C8; %k[27] = 0xBF597FC7;
    %k[28] = 0xC6E00BF3; %k[29] = 0xD5A79147; %k[30] = 0x06CA6351; %k[31] = 0x14292967;
    %k[32] = 0x27B70A85; %k[33] = 0x2E1B2138; %k[34] = 0x4D2C6DFC; %k[35] = 0x53380D13;
    %k[36] = 0x650A7354; %k[37] = 0x766A0ABB; %k[38] = 0x81C2C92E; %k[39] = 0x92722C85;
    %k[40] = 0xA2BFE8A1; %k[41] = 0xA81A664B; %k[42] = 0xC24B8B70; %k[43] = 0xC76C51A3;
    %k[44] = 0xD192E819; %k[45] = 0xD6990624; %k[46] = 0xF40E3585; %k[47] = 0x106AA070;
    %k[48] = 0x19A4C116; %k[49] = 0x1E376C08; %k[50] = 0x2748774C; %k[51] = 0x34B0BCB5;
    %k[52] = 0x391C0CB3; %k[53] = 0x4ED8AA4A; %k[54] = 0x5B9CCA4F; %k[55] = 0x682E6FF3;
    %k[56] = 0x748F82EE; %k[57] = 0x78A5636F; %k[58] = 0x84C87814; %k[59] = 0x8CC70208;
    %k[60] = 0x90BEFFFA; %k[61] = 0xA4506CEB; %k[62] = 0xBEF9A3F7; %k[63] = 0xC67178F2;
    
    // pre-processing:
    %len = strLen(%text);
    for (%i = 0; %i < %len; %i++)
      %byte[%i] = strPos($ASCIITable, getSubStr(%text, %i, 1)) + 1;
    
    // append a single 1 bit to the end of the original message,
    // then 0 bits to pad the message to 64 bits less than a full chunk
    %byte[%len] = 128;
    %lPos = (%len + 8 | 63) - 7 | 0;
    for (%i = %len + 1; %i < %lPos; %i++)
      %byte[%i] = 0;
    
    // append the length of the original message, in bits, to the end of the message
    // the length is appended as a 64-bit big-endian integer
    %bitLen = %len << 3;
    for (%i = 0; %i < 8; %i++)
    {
      %byte[%lPos + 7 - %i] = %bitLen & 255;
      %bitLen >>= 8;
    }
    %len = %lPos + 8 | 0;
    
    // convert the bytes to 32-bit words
    %wLen = %len >> 2;
    for (%i = 0; %i < %wLen; %i++)
    {
      %bPos = %i << 2;
      for (%j = 0; %j < 4; %j++)
        %word[%i] = %word[%i] << 8 | %byte[%bPos + %j];
    }
    
    // process the message in 512-bit chunks (512 / 32 = 16, so %wLen >> 4):
    %chunks = %wLen >> 4;
    for (%chunk = 0; %chunk < %chunks; %chunk++)
    {
      // copy the current chunk to the beginning of the message schedule array
      %offset = %chunk << 4;
      for (%i = 0; %i < 16; %i++)
        %w[%i] = %word[%i + %offset];
      
      // extend the first 16 words into the remaining 48 words
      for (%i = 16; %i < 64; %i++)
      {
        %s0 = (%w[%i - 15] >> 7 | %w[%i - 15] << 25) ^ (%w[%i - 15] >> 18 | %w[%i - 15] << 14) ^ %w[%i - 15] >> 3;
        %s1 = (%w[%i - 2] >> 17 | %w[%i - 2] << 15) ^ (%w[%i - 2] >> 19 | %w[%i - 2] << 13) ^ %w[%i - 2] >> 10;
        
        // inline version of Port's add function: https://forum.blockland.us/index.php?topic=248922.0
        // We have to avoid native addition for this because TorqueScript cannot do math. :(
        // Operation: %w[%i] = %w[%i - 16] + %s0 + %w[%i - 7] + %s1;
        %add0 = %w[%i - 16];
        %add1 = %s0;
        %add2 = %w[%i - 7];
        %add3 = %s1;
        for (%j = 0; %j < 3; %j++)
        {
          %_a = 1;
          %_x = %add[%j];
          %_y = %add[%j + 1];
          while (%_a)
          {
            %_a = %_x & %_y;
            %_b = %_x ^ %_y;
            %_x = %_a << 1;
            %_y = %_b;
          }
          %add[%j + 1] = %_b;
        }
        %w[%i] = %_b;
      }
      
      // initialize working variables
      %a = %h0;
      %b = %h1;
      %c = %h2;
      %d = %h3;
      %e = %h4;
      %f = %h5;
      %g = %h6;
      %h = %h7;
      
      // main compression function
      // the "%i < 64" here controls the number of rounds
      for (%i = 0; %i < 64; %i++)
      {
        %s1 = (%e >> 6 | %e << 26) ^ (%e >> 11 | %e << 21) ^ (%e >> 25 | %e << 7);
        %ch = (%e & %f) ^ (~%e & %g);
        
        // inline Port add
        // Operation: %temp1 = %h + %s1 + %ch + %k[%i] + %w[%i]
        %add0 = %h;
        %add1 = %s1;
        %add2 = %ch;
        %add3 = %k[%i];
        %add4 = %w[%i];
        for (%j = 0; %j < 4; %j++)
        {
          %_a = 1;
          %_x = %add[%j];
          %_y = %add[%j + 1];
          while (%_a)
          {
            %_a = %_x & %_y;
            %_b = %_x ^ %_y;
            %_x = %_a << 1;
            %_y = %_b;
          }
          %add[%j + 1] = %_b;
        }
        %temp1 = %_b;
        %s0 = (%a >> 2 | %a << 30) ^ (%a >> 13 | %a << 19) ^ (%a >> 22 | %a << 10);
        %maj = (%a & %b) ^ (%a & %c) ^ (%b & %c);
        
        // inline Port add
        // Operation: %temp2 = %s0 + %maj;
        %_a = 1;
        while (%_a)
        {
          %_a = %s0 & %maj;
          %_b = %s0 ^ %maj;
          %s0 = %_a << 1;
          %maj = %_b;
        }
        %temp2 = %_b;
        
        %h = %g;
        %g = %f;
        %f = %e;
        
        // inline Port add
        // Operation: %e = %d + %temp1;
        %_a = 1;
        %_x = %temp1;
        while (%_a)
        {
          %_a = %_x & %d;
          %_b = %_x ^ %d;
          %_x = %_a << 1;
          %d = %_b;
        }
        %e = %_b;
        %d = %c;
        %c = %b;
        %b = %a;
        
        // inline Port add
        // Operation: %a = %temp1 + %temp2;
        %_a = 1;
        while (%_a)
        {
          %_a = %temp1 & %temp2;
          %_b = %temp1 ^ %temp2;
          %temp1 = %_a << 1;
          %temp2 = %_b;
        }
        %a = %_b;
      }
      
      // increment %h[0] through %h[7] by %a through %h
      // we can't do this more simply by using %h[0] += %a; because TorqueScript cannot add
      %v0 = %a; %v1 = %b; %v2 = %c; %v3 = %d;
      %v4 = %e; %v5 = %f; %v6 = %g; %v7 = %h;
      for (%i = 0; %i < 8; %i++)
      {
        // inline Port add
        // Operation: %h[%i] = %h[%i] + %v[%i];
        %_a = 1;
        %_x = %h[%i];
        %_y = %v[%i];
        while (%_a)
        {
          %_a = %_x & %_y;
          %_b = %_x ^ %_y;
          %_x = %_a << 1;
          %_y = %_b;
        }
        %h[%i] = %_b;
      }
    }
    
    // produce hexadecimal string and return it
    %hex = "0123456789abcdef";
    for (%i = 0; %i < 8; %i++)
    {
      %word = "";
      for (%j = 0; %j < 8; %j++)
      {
        %word = getSubStr(%hex, %h[%i] & 15, 1) @ %word;
        %h[%i] >>= 4;
      }
      %o = %o @ %word;
    }
    return %o;
  }
};