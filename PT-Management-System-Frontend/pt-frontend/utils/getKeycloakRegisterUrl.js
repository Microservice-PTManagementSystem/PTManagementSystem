export function getKeycloakRegisterUrl() {
  const base = process.env.NEXT_PUBLIC_KEYCLOAK_BASEURL ?? "http://localhost:8080";
  const realm = process.env.NEXT_PUBLIC_KEYCLOAK_REALM ?? "ptmanagement";
  const clientId = process.env.NEXT_PUBLIC_KEYCLOAK_CLIENT_ID ?? "nextjs-app";

  const redirect = encodeURIComponent(
    `${process.env.NEXT_PUBLIC_APP_URL ?? "http://localhost:3000"}`
  );

 
  const role = typeof window !== "undefined" ? localStorage.getItem("selectedRole") : null;

  const url = new URL(`${base}/realms/${realm}/protocol/openid-connect/registrations`);
  url.searchParams.set("client_id", clientId);
  url.searchParams.set("response_type", "code");
  url.searchParams.set("scope", "openid");
  url.searchParams.set("redirect_uri", decodeURIComponent(redirect));

  if (role) {
    url.searchParams.set("login_hint", role);
  }

  return url.toString();
}

  
  export const openKeycloakRegister = () => {
    window.location.href = getKeycloakRegisterUrl();
  };
  