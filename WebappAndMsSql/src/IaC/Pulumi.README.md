# README

## デプロイ
```bash
pulumi up
```

## 削除
```bash
pulumi destroy
```

## prodスタックの作成
```bash
pulumi stack init prod --copy-config-from dev
```

## prodスタックの選択
```bash
pulumi stack select prod
```

## prodスタックのデプロイ
```bash
pulumi up
```
