
export function getKeycloakRegisterUrl() {
    const base      = process.env.NEXT_PUBLIC_KEYCLOAK_BASEURL  ?? "http://localhost:8080";
    const realm     = process.env.NEXT_PUBLIC_KEYCLOAK_REALM    ?? "ptmanagement";
    const clientId  = process.env.NEXT_PUBLIC_KEYCLOAK_CLIENT_ID?? "nextjs-app";
  
    const redirect  =
      encodeURIComponent(
        `${process.env.NEXT_PUBLIC_APP_URL ?? "http://localhost:3000"}`
      );
  
    return (
      `${base}/realms/${realm}/protocol/openid-connect/registrations` +
      `?client_id=${clientId}` +
      `&response_type=code` +
      `&scope=openid` +
      `&redirect_uri=${redirect}`
    );
  }
  
  export const openKeycloakRegister = () => {
    window.location.href = getKeycloakRegisterUrl();
  };
  