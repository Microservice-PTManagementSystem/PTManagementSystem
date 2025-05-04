import NextAuth from "next-auth";
import KeycloakProvider from "next-auth/providers/keycloak";

export default NextAuth({
  providers: [
    KeycloakProvider({
      clientId: process.env.KEYCLOAK_CLIENT_ID,
      clientSecret: process.env.KEYCLOAK_CLIENT_SECRET,
      issuer: process.env.KEYCLOAK_ISSUER, 
      profile(profile, tokens) {
        console.log("PROFILE CALLBACK:", profile); // Bu server log'unda çıkar
        return {
          id: profile.sub,
          name: profile.preferred_username,
          email: profile.email,
        };
      },
    }),
  ],
  debug:true,
  secret: process.env.NEXTAUTH_SECRET,
  callbacks: {
    async jwt({ token, account,profile }) {
      if (account && profile) {
        token.accessToken = account.access_token;
        token.idToken = account.id_token;
        token.id = profile?.sub; 
      }
      console.log("JWT CALLBACK - PROFILE:", profile);

      return token;
    },
    async session({ session, token }) {
      session.accessToken = token.accessToken;
      session.idToken = token.idToken;
      session.user.id = token.id;
      return session;
    },
        
  },
});
