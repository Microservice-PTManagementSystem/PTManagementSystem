import NextAuth from "next-auth";
import KeycloakProvider from "next-auth/providers/keycloak";

export default NextAuth({
  providers: [
    KeycloakProvider({
      clientId: process.env.KEYCLOAK_CLIENT_ID,
      clientSecret: process.env.KEYCLOAK_CLIENT_SECRET,
      issuer: process.env.KEYCLOAK_ISSUER, 
      profile(profile, tokens) {
        console.log("PROFILE CALLBACK:", profile); 
        return {
          id: profile.sub,
          name: profile.name,
          username:profile.preferred_username,
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
        token.name = profile?.name;
        token.username = profile?.email?.split("@")[0] || "";
        token.email = profile?.email;
      }
      console.log("JWT CALLBACK - PROFILE:", profile);
      console.log("JWT CALLBACK -:", token.accessToken);

      return token;
    },
    async session({ session, token }) {
      session.accessToken = token.accessToken;
      session.idToken = token.idToken;
      session.user.id = token.id;
      session.user.name = token.name;
      session.user.username = token.username;
      session.user.email = token.email;
      return session;
    },
        
  },
});
