- Dependabot 2.0 + package lock files
- SBOM generation via .NET SDK -> Microsoft.Sbom.Targets

- Attestations / signing
  - cosign
- openssf

```
docker run --rm \
  -e TRIVY_DB_REPOSITORY=public.ecr.aws/aquasecurity/trivy-db,aquasec/trivy-db,ghcr.io/aquasecurity/trivy-db \
  -v $(pwd):/workspace \
  aquasec/trivy fs \
  --scanners license,vuln,misconfig,secret --format cosign-vuln \
  --license-full \
  /workspace
```